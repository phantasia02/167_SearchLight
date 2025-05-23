﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fragments from demolition
// Scale support for bound + Unyielding component

namespace RayFire
{
    [DisallowMultipleComponent]
    [AddComponentMenu ("RayFire/Rayfire Connectivity")]
    [HelpURL ("https://rayfirestudios.com/unity-online-help/components/unity-connectivity-component/")]
    public class RayfireConnectivity : MonoBehaviour
    {
        public enum RFConnInitType
        {
            AtStart     = 1,
            ByMethod    = 3,
            ByIntegrity = 5
        }
        
        public ConnectivityType type;
        public float            expand;
        public float            minimumArea;
        public float            minimumSize;
        public int              percentage;
        public int              seed;
        public bool             clusterize = true;
        public bool             demolishable;
        public RFConnInitType   startCollapse       = RFConnInitType.ByMethod;
        public int              collapseByIntegrity = 50;
        public RFCollapse       collapse;
        public RFConnInitType   startStress       = RFConnInitType.ByMethod;
        public int              stressByIntegrity = 70;
        public RFStress         stress;
        
        public bool showConnections = true;
        public bool showNodes       = true;
        public bool showGizmo       = true;
        public bool showStress;
        
        public bool               checkConnectivity;
        public bool               connectivityCheckNeed;
        public List<RayfireRigid> rigidList;
        public RFCluster          cluster;
        public int                initShardAmount;
        public int                clsCount;
        public RayfireRigidRoot   rigidRootHost;
        public RayfireRigid       meshRootHost;
        
        [NonSerialized] public RFBackupCluster backup;
        [NonSerialized] public bool            childrenChanged;
        [NonSerialized]        bool            childrenCorState;
        [NonSerialized]        bool            connectivityCorState;
        [NonSerialized]        bool            corState;

        /// /////////////////////////////////////////////////////////
        /// Common
        /// ///////////////////////////////////////////////////////// 

        // Set hosts at init if applied in runtime
        void SetRuntimeMeshRootHost()
        {
            if (meshRootHost == null && rigidRootHost == null)
            {
                meshRootHost = GetComponent<RayfireRigid>();
                if (meshRootHost != null)
                {
                    if (meshRootHost.objectType == ObjectType.MeshRoot)
                        meshRootHost.activation.connect = this;
                    else
                        meshRootHost = null;
                }
            }
        }

        /// /////////////////////////////////////////////////////////
        /// Reset
        /// /////////////////////////////////////////////////////////
        
        // Reset    
        public void ResetConnectivity()
        {
            childrenCorState     = false;
            connectivityCorState = false;
            collapse.inProgress  = false;
            stress.inProgress    = false;
            clsCount             = 1;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Enable/Disable
        /// /////////////////////////////////////////////////////////
        
        // Disable
        void OnDisable()
        {
            corState             = false;
            childrenCorState     = false;
            connectivityCorState = false;
        }

        // Enable
        void OnEnable()
        {
            // Start cors
            if (gameObject.activeSelf == true && cluster != null && 
                cluster.shards != null && cluster.shards.Count > 0 &&
                corState == false)
            {
                // Init connectivity coroutines
                StartCoroutine(ChildrenCor());
                StartCoroutine(ConnectivityCor());
                
                // Continue collapse
                if (collapse.inProgress == true)
                {
                    collapse.inProgress = false;
                    RFCollapse.StartCollapse (this);
                }

                // Continue stress
                if (stress.enable  == true)
                {
                    if (stress.inProgress == true)
                    {
                        stress.inProgress = false;
                        RFStress.StartStress (this);
                    }
                }
                
                // All coroutines are running
                corState = true;
            }
        }
        
        /// /////////////////////////////////////////////////////////
        /// Initialize
        /// /////////////////////////////////////////////////////////
        
        // Initialize, Only for child Rigids
        public void Initialize()
        {
            // Set hosts at init if applied in runtime
            SetRuntimeMeshRootHost();
            
            // Check
            if (WarningCheck() == false)
                return;
            
            // Set rigidList if has meshRoot host
            if (meshRootHost != null)
            {
                // Set connections by meshRoot fragments list
                SetClusterForMeshRoot();
                
                // RigidList check
                if (rigidList.Count == 0)
                {
                    ResetSetup();
                    Debug.Log ("RayFire Connectivity: " + name + " has no objects to check for connectivity. Connectivity disabled.", gameObject);
                    return;
                }
            }

            // Set shardList if has rigidRoot host
            if (rigidRootHost != null)
                SetClusterForRigidRoot();
            
            // Start all coroutines in playmode. Do not if editor setup
            if (Application.isPlaying == true)
                StartAllCoroutines();
        }

        // Check
        bool WarningCheck()
        {
            // Check for not meshRoot rigid
            if (meshRootHost != null)
            {
                // Not mesh root rigid
                if (meshRootHost.objectType != ObjectType.MeshRoot)
                {
                    Debug.Log ("RayFire Connectivity: " + name + " object has Rigid host but it's object type is not Mesh Root. Connectivity disabled.", gameObject);
                    return false;
                }

                // Disabled by connectivity
                if (meshRootHost.activation.byConnectivity == false)
                {
                    Debug.Log ("RayFire Connectivity: " + name + " object has Rigid host with Mesh Root type but activation By Connectivity is disabled. Connectivity disabled.", gameObject);
                    return false;
                }
            }
            
            // Check for not rigidRoot
            if (rigidRootHost != null)
            {
                // Disabled by connectivity
                if (rigidRootHost.activation.byConnectivity == false)
                {
                    Debug.Log ("RayFire Connectivity: " + name + " object has RigidRoot host but activation By Connectivity is disabled. Connectivity disabled.", gameObject);
                    return false;
                }
            }
            
            return true;
        }
        
        // Start all coroutines
        public void StartAllCoroutines()
        {
            // Start cors
            StartCoroutine(ChildrenCor());
            StartCoroutine(ConnectivityCor());
            
            // Init collapse
            if (startCollapse == RFConnInitType.AtStart)
                RFCollapse.StartCollapse(this);
            
            // Init stress
            if (stress != null && stress.enable == true)
                if (startStress == RFConnInitType.AtStart)
                    RFStress.StartStress(this);
            
            // All coroutines are running
            corState = true;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Cluster Common
        /// /////////////////////////////////////////////////////////  
        
        // Prepare cluster
        void PrepareCluster()
        {
            // In case of runtime add
            if (cluster == null)
                cluster = new RFCluster();

            // Missing shards check in case of cached shards
            if (RFCluster.IntegrityCheck (cluster) == false)
            {
                Debug.Log ("RayFire Connectivity: " + name + " object has missing shards. Reset and Editor Setup Connectivity host again.", gameObject);
                cluster            = new RFCluster();
                stress.initialized = false;
                // TODO stress / support list shards reset
            }
                        
            // Cluster props
            cluster.demolishable = demolishable;
                
            // Cluster amount
            clsCount = 1;
        }

        // Create default cluster
        void CreateCluster()
        {
            cluster              = new RFCluster();
            cluster.id           = RFCluster.GetUniqClusterId (cluster);
            cluster.tm           = transform;
            cluster.depth        = 0;
            cluster.pos          = transform.position;
            cluster.rot          = transform.rotation;
            cluster.demolishable = demolishable;
            cluster.initialized  = true;
        }

        /// /////////////////////////////////////////////////////////
        /// Set Cluster by Rigids
        /// /////////////////////////////////////////////////////////  
        
        // Set cluster
        void SetClusterForMeshRoot ()
        {
            // Prepare cluster. Check for integrity of cached shards.
            PrepareCluster();

            // Set rigids list and connect with Connectivity component
            SetRigidListByFragments (meshRootHost.fragments);
            
            // Play mode ops. Not for Editor. Shards were cached, reinit non serialized vars, clear list otherwise
            if (Application.isPlaying == true)
                if (InitCachedShardsByRigidList (rigidList, cluster) == true)
                    cluster.shards.Clear();
            
            // Create main cluster
            if (cluster.shards.Count == 0)
            {
                // Create default cluster
                CreateCluster();
                
                // Set shards for main cluster
                RFShard.SetShardsByRigidList (cluster, rigidList, type);

                // Set expand
                SetExpand();
                
                // Set shard neibs
                RFShard.SetShardNeibs (cluster.shards, type, minimumArea, minimumSize, percentage, seed);
            
                // Set range for area and size
                RFCollapse.SetRangeData (cluster, percentage, seed);
            }
            
            // Set stress
            if (stress != null && stress.enable == true)
                RFStress.Initialize(this);
            
            // Set backup cluster
            RFBackupCluster.BackupConnectivity (this);
            
            // Set initial shards amount
            initShardAmount = cluster.shards.Count;
        }

        // Set cluster
        void SetClusterForRigidRoot ()
        {
           
            // Prepare cluster
            PrepareCluster();

            // Cached RigidRoot runtime init. Reinit non serialized vars, clear list otherwise
            if (Application.isPlaying == true && cluster.cachedHost == true)
                InitCachedShardsByRigidRoot (rigidRootHost, cluster);

            // Create main cluster and set neibs. If Editor Setup or clean runtime init
            if (cluster.cachedHost == false)
            {
                // Create default cluster
                CreateCluster();
                
                // Mirror shards from RigidRoot for main cluster
                for (int i = 0; i < rigidRootHost.cluster.shards.Count; i++)
                {
                    rigidRootHost.cluster.shards[i].cluster = cluster;
                    cluster.shards.Add(rigidRootHost.cluster.shards[i]);
                }

                // Set expand
                SetExpand();

                // Set faces data for connectivity
                if (type == ConnectivityType.ByMesh || type == ConnectivityType.ByBoundingBoxAndMesh)
                    for (int i = 0; i < cluster.shards.Count; i++)
                        RFTriangle.SetTriangles(cluster.shards[i], cluster.shards[i].mf); 
                
                // Set shard neibs
                RFShard.SetShardNeibs (cluster.shards, type, minimumArea, minimumSize, percentage, seed);
                
                // Set range for area and size
                RFCollapse.SetRangeData (cluster, percentage, seed);

                // Shard connections are cached and stored in host as well
                cluster.cachedHost = true;
            }

            // Set stress
            if (stress.enable == true)
                RFStress.Initialize(this);
                        
            // Set backup cluster
            RFBackupCluster.BackupConnectivity (this);
            
            // Set initial shards amount
            initShardAmount = cluster.shards.Count;
        }

        // Set bound expand for shards for better BBOX connectivity
        void SetExpand()
        {
            if (expand > 0)
                if (type == ConnectivityType.ByBoundingBox || type == ConnectivityType.ByBoundingBoxAndMesh)
                    for (int i = 0; i < cluster.shards.Count; i++)
                        cluster.shards[i].bnd.Expand (expand);
        }
        
        /// /////////////////////////////////////////////////////////
        /// Setup rigidList without host
        /// ///////////////////////////////////////////////////////// 

        // Set rigidList
        void SetRigidListByFragments(List<RayfireRigid> rgList)
        {
            // No targets
            if (rgList.Count == 0)
                return;

            // Get rigid with byConnectivity
            rigidList = new List<RayfireRigid>();
            for (int i = 0; i < rgList.Count; i++)
                if (rgList[i].simulationType == SimType.Inactive || rgList[i].simulationType == SimType.Kinematic)
                    if (rgList[i].activation.byConnectivity == true)
                        rigidList.Add (rgList[i]);

            // No targets
            if (rigidList.Count == 0)
                return;

            // Set this connectivity as main connectivity node
            for (int i = 0; i < rigidList.Count; i++)
                rigidList[i].activation.connect = this;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Reinit shards in case of cached prefab
        /// ///////////////////////////////////////////////////////// 
        
        // Reinit shard's non serialized fields in case of prefab use
        public static bool InitCachedShardsByRigidList (List<RayfireRigid> rigids, RFCluster cluster)
        {
            // Not initialized
            if (cluster.initialized == true)
                return false;
            
            // No shards
            if (cluster.shards.Count == 0)
                return false;
            
            // Rigid list doesn't match shards. TODO compare per shard
            if (cluster.shards.Count != rigids.Count)
                return true;
            
            // Reinit
            for (int i = 0; i < cluster.shards.Count; i++)
            {
                if (rigids[i] != null)
                {
                    cluster.shards[i].uny   = rigids[i].activation.unyielding;
                    cluster.shards[i].act   = rigids[i].activation.activatable;
                    cluster.shards[i].sm    = rigids[i].simulationType;
                    
                    cluster.shards[i].col   = rigids[i].physics.meshCollider;
                    cluster.shards[i].rigid = rigids[i];
                }
                
                cluster.shards[i].cluster    = cluster;
                
                if (cluster.shards[i].neibShards == null)
                    cluster.shards[i].neibShards = new List<RFShard>();
                else
                    cluster.shards[i].neibShards.Clear();
                
                for (int n = 0; n < cluster.shards[i].nIds.Count; n++)
                    cluster.shards[i].neibShards.Add (cluster.shards[cluster.shards[i].nIds[n]]);
            }

            cluster.initialized = true;
            return false;
        }
        
        // Reinit shard's non serialized fields in case of prefab use
        public static void InitCachedShardsByRigidRoot (RayfireRigidRoot root, RFCluster cluster)
        {
            // Set shards for main cluster
            cluster.shards.Clear();
            for (int i = 0; i < root.cluster.shards.Count; i++)
            {
                root.cluster.shards[i].cluster = cluster;
                cluster.shards.Add(root.cluster.shards[i]);
            }

            // Reinit neibShards
            for (int i = 0; i < cluster.shards.Count; i++)
            {
                if (cluster.shards[i].neibShards == null)
                    cluster.shards[i].neibShards = new List<RFShard>();
                else
                    cluster.shards[i].neibShards.Clear();
                
                for (int n = 0; n < cluster.shards[i].nIds.Count; n++)
                    cluster.shards[i].neibShards.Add (cluster.shards[cluster.shards[i].nIds[n]]);
            }
            cluster.initialized = true;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Connectivity Cor
        /// /////////////////////////////////////////////////////////   
        
        // Connectivity check cor
        IEnumerator ConnectivityCor()
        {
            // Stop if running 
            if (connectivityCorState == true)
                yield break;
            
            // Set running state
            connectivityCorState = true;
            
            checkConnectivity = true;
            while (checkConnectivity == true)
            {
                // Child deleted
                if (childrenChanged == true)
                    ChildrenCheck();

                // Get not connected groups
                if (connectivityCheckNeed == true)
                    CheckConnectivity();

                yield return null;
            }
            
            // Set state
            connectivityCorState = false;
        }
        
        // Check for connectivity
        public void CheckConnectivity()
        {
            // Rigid Root connectivity check.
            if (rigidRootHost != null)
            {
                CheckConnectivityRigidRoot();
                return;
            }

            // Check for connectivity
            CheckConnectivityRigidList();

            // Start collapse by integrity
            if (startCollapse == RFConnInitType.ByIntegrity)
                if (AmountIntegrity < collapseByIntegrity)
                    RFCollapse.StartCollapse(this);
            
            // Start stress by integrity
            if (startStress == RFConnInitType.ByIntegrity)
                if (AmountIntegrity < stressByIntegrity)
                    RFStress.StartStress(this);
        }
       
        /// /////////////////////////////////////////////////////////
        /// Connectivity check
        /// /////////////////////////////////////////////////////////  
        
        // Check for connectivity
        void CheckConnectivityRigidList()
        {
            // Do once
            connectivityCheckNeed = false;

            // Clear all activated/demolished shards
            CleanUpActivatedShardsRigidList (cluster);

            // No shards to check
            if (cluster.shards.Count == 0)
                return;
 
            // Reinit neibs after cleaning
            RFShard.ReinitNeibs (cluster.shards);

            // TODO do not collect solo uny shards
            
            // Check for solo shards and collect to activate
            List<RFShard> soloShards = new List<RFShard>(); // TODO make local private field
            RFCluster.GetSoloShards (cluster, soloShards);
            
            // Separate all not connected groups to child clusters
            RFCluster.ConnectivityCheck (cluster);
            
            // TODO all shards connected and has no uny shards, should be activated
            // do not because no child clusters
            
            // Get not connected and not unyielding child cluster
            CheckUnyieldingRigidList (cluster);

            // TODO ONE NEIB DETACH FOR CHILD CLUSTERS
            
            // Activate solo shards or/and clusterize not connected groups
            ActivateShards (soloShards);
            
            // Stop checking. Everything activated
            if (cluster.shards.Count == 0)
                checkConnectivity = false;
        }
        
        // Check for connectivity
        void CheckConnectivityRigidRoot()
        {
            // Do once
            connectivityCheckNeed = false;
 
            // Clear all activated/demolished shards
            CleanUpActivatedShardsRigidRoot (cluster);

            // No shards to check
            if (cluster.shards.Count == 0)
                return;

            // Reinit neibs after cleaning
            RFShard.ReinitNeibs (cluster.shards);

            // TODO do not collect solo uny shards
            
            // Check for solo shards and collect
            List<RFShard> soloShards = new List<RFShard>();
            RFCluster.GetSoloShards (cluster, soloShards);

            // Connectivity check TODO new cluster id fail
            RFCluster.ConnectivityCheck (cluster);
            
            // Get not connected and not unyielding child cluster
            CheckUnyieldingRigidRoot (cluster);

            // TODO ONE NEIB DETACH FOR CHILD CLUSTERS
            
            // Activate shards or clusterize not connected groups
            ActivateShards (soloShards);

            // Stop checking. Everything activated
            if (cluster.shards.Count == 0)
                checkConnectivity = false;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Methods
        /// ///////////////////////////////////////////////////////// 
        
        // Activate solo shards or/and clusterize not connected groups
        void ActivateShards(List<RFShard> soloShards)
        {
            // Activate not connected shards. 
            if (soloShards.Count > 0)
                for (int i = 0; i < soloShards.Count; i++)
                    RFActivation.ActivateShard (soloShards[i], rigidRootHost);
            
            // Clusterize childClusters or activate their shards
            if (cluster.HasChildClusters == true)
            {
                if (clusterize == true)
                    Clusterize();
                else
                    for (int c = 0; c < cluster.childClusters.Count; c++)
                        for (int s = 0; s < cluster.childClusters[c].shards.Count; s++)
                            RFActivation.ActivateShard (cluster.childClusters[c].shards[s], rigidRootHost);
            }
        }
        
        // Clusterize not connected groups
        void Clusterize()
        {
            for (int i = 0; i < cluster.childClusters.Count; i++)
            {
                // Activatable unyielding activated with 0 shards left
                if (cluster.childClusters[i].shards.Count == 0)
                    return;
                
                // Set bound 
                cluster.childClusters[i].bound = RFCluster.GetShardsBound (cluster.childClusters[i].shards);
                
                // Create root for left children
                if (cluster.childClusters[i].tm == null)
                    RFCluster.CreateClusterRoot (cluster.childClusters[i], cluster.childClusters[i].shards[0].tm.position, 
                        Quaternion.identity, gameObject.layer, gameObject.tag, gameObject.name + RFDemolitionCluster.nameApp + clsCount++);  
                
                // Add Connected cluster Rigid
                cluster.childClusters[i].rigid = cluster.childClusters[i].tm.gameObject.AddComponent<RayfireRigid>();
                
                // RigidRoot
                if (rigidRootHost != null)
                {
                    // Copy Rigid properties
                    rigidRootHost.CopyPropertiesTo (cluster.childClusters[i].rigid);
                    
                    // Copy particles
                    RFParticles.CopyRigidRootParticles (rigidRootHost, cluster.childClusters[i].rigid);
                    
                    // Copy Sound
                    RFSound.CopySound (rigidRootHost.sound, cluster.childClusters[i].rigid);
                    
                    // Per shard ops
                    for (int s = 0; s < cluster.childClusters[i].shards.Count; s++)
                    {
                        // Set dynamic state
                        cluster.childClusters[i].shards[s].sm = SimType.Dynamic;
                        
                        // Should not be faded because of offset fade
                        cluster.childClusters[i].shards[s].fade = -1;
                        
                        // Destroy rigidbody component
                        Destroy (cluster.childClusters[i].shards[s].rb);
                    }
                    
                    // Collect child cluster
                    // rigidRootHost.clusters.Add (cluster.childClusters[i]);
                    
                    // TODO Clean Up from shards
                }
                
                // Rigid List
                if (meshRootHost != null)
                {
                    // Copy Rigid properties
                    cluster.childClusters[i].shards[0].rigid.CopyPropertiesTo (cluster.childClusters[i].rigid);

                    // Copy particles
                    RFParticles.CopyRigidParticles (meshRootHost, cluster.childClusters[i].rigid);
                    
                    // Copy Sound
                    RFSound.CopySound (meshRootHost.sound, cluster.childClusters[i].rigid);
                    
                    // Destroy components
                    for (int s = 0; s < cluster.childClusters[i].shards.Count; s++)
                    {
                        // Stop all rigid coroutines
                        cluster.childClusters[i].shards[s].rigid.StopAllCoroutines();
                        
                        // Destroy rigidbody
                        Destroy (cluster.childClusters[i].shards[s].rigid.physics.rigidBody);
                    }
                }
                
                // Set connectivity cluster as main cluster for child clusters to transfer later to demolished connected clusters
                cluster.childClusters[i].mainCluster = cluster;
                
                // Set Rigid properties
                cluster.childClusters[i].rigid.objectType           = ObjectType.ConnectedCluster;
                cluster.childClusters[i].rigid.clusterDemolition.cn = showConnections;
                cluster.childClusters[i].rigid.clusterDemolition.nd = showNodes;

                // Create connected cluster
                RFDemolitionCluster.CreateClusterRuntime (cluster.childClusters[i].rigid, cluster.childClusters[i]);

                // Activation fade
                if (cluster.childClusters[i].rigid.fading.onActivation == true)
                    cluster.childClusters[i].rigid.Fade();
            }
        }

        // Clear all activated/demolished shards TODO change to CleanUpActivatedShardsRigidRoot method:sm check
        static void CleanUpActivatedShardsRigidList(RFCluster cluster)
        {
            for (int i = cluster.shards.Count - 1; i >= 0; i--)
            {
                if (cluster.shards[i].rigid == null ||
                    cluster.shards[i].rigid.activation.connect == null ||
                    cluster.shards[i].rigid.limitations.demolished == true)
                {
                    cluster.shards[i].cluster = null;
                    cluster.shards.RemoveAt (i);
                }
            }
        }

        // Collect solo shards, remove from cluster, reinit cluster
        static void CheckUnyieldingRigidList(RFCluster cluster)
        {
            // Has no child clusters, but main cluster has no Uny shards. Send to child cluster
            if (cluster.HasChildClusters == false && cluster.UnyieldingByShard == false)
            {
                // Create new cluster by shards
                RFCluster.NewClusterByShards (cluster, cluster.shards);

                // Clear shards                
                cluster.shards.Clear();
            }
            
            // Get not connected and not unyielding child cluster
            if (cluster.HasChildClusters == true)
            {
                // Remove all unyielding child clusters
                for (int c = cluster.childClusters.Count - 1; c >= 0; c--)
                {
                    if (cluster.childClusters[c].UnyieldingByRigid == true)
                    {
                        cluster.shards.AddRange (cluster.childClusters[c].shards);
                        cluster.childClusters.RemoveAt (c);
                    }
                }
                
                // Set unyielding cluster shards back to original cluster
                for (int s = 0; s < cluster.shards.Count; s++)
                    cluster.shards[s].cluster = cluster;
            }
        }

        // Clear all activated/demolished shards
        static void CleanUpActivatedShardsRigidRoot(RFCluster cluster)
        {
            for (int i = cluster.shards.Count - 1; i >= 0; i--)
            {
                // TODO check for deactivated/destroyed
                if (cluster.shards[i].sm == SimType.Dynamic)
                {
                    cluster.shards[i].cluster = null;
                    cluster.shards.RemoveAt (i);
                }
            }
        }
        
        // Collect solo shards, remove from cluster, reinit cluster
        static void CheckUnyieldingRigidRoot(RFCluster cluster)
        {
            // Has no child clusters, but main cluster has no Uny shards. Send to child cluster
            if (cluster.HasChildClusters == false && cluster.UnyieldingByShard == false)
            {
                // Create new cluster by shards
                RFCluster.NewClusterByShards (cluster, cluster.shards);

                // Clear shards                
                cluster.shards.Clear();
            }

            // Get not connected and not unyielding child cluster
            if (cluster.HasChildClusters == true)
            {
                // Remove all unyielding child clusters
                for (int c = cluster.childClusters.Count - 1; c >= 0; c--)
                {
                    if (cluster.childClusters[c].UnyieldingByShard == true)
                    {
                        cluster.shards.AddRange (cluster.childClusters[c].shards);
                        cluster.childClusters.RemoveAt (c);
                    }
                }
                
                // Set unyielding cluster shards back to original cluster
                for (int s = 0; s < cluster.shards.Count; s++)
                    cluster.shards[s].cluster = cluster;
            }
        }
        
        /// /////////////////////////////////////////////////////////
        /// Children change
        /// /////////////////////////////////////////////////////////    
        
        // Child removed
        void OnTransformChildrenChanged()
        {
            childrenChanged = true;
            //integrityCheck  = true;
        }
        
        // Connectivity check cor
        IEnumerator ChildrenCor()
        {
            // Stop if running 
            if (childrenCorState == true)
                yield break;
            
            // Set running state
            childrenCorState = true;
            
            bool checkChildren = true;
            while (checkChildren == true)
            {
                // Get not connected groups
                if (childrenChanged == true)
                    connectivityCheckNeed = true;

                yield return null;
            }
            
            // Set state
            childrenCorState = false;
        }

        // Check for children
        void ChildrenCheck()
        {
            for (int s = cluster.shards.Count - 1; s >= 0; s--)
            {
                if (cluster.shards[s].tm == null)
                {
                    if (cluster.shards[s].neibShards.Count > 0)
                    {
                        // Remove itself in neibs
                        for (int n = 0; n < cluster.shards[s].neibShards.Count; n++)
                        {
                            // Check every neib in neib
                            for (int i = 0; i < cluster.shards[s].neibShards[n].neibShards.Count; i++)
                            {
                                if (cluster.shards[s].neibShards[n].neibShards[i] == cluster.shards[s])
                                {
                                    cluster.shards[s].neibShards[n].RemoveNeibAt (i);
                                    break;
                                }
                            }
                        }
                    }
                    cluster.shards.RemoveAt (s);
                }
            }
            childrenChanged = false;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Reset Setup
        /// ///////////////////////////////////////////////////////// 

        // Reset cluster data
        public void ResetSetup()
        {
            rigidRootHost      = null;
            meshRootHost       = null;
            cluster            = new RFCluster();
            stress.strShards   = new List<RFShard>();
            stress.supShards   = new List<RFShard>();
            stress.initialized = false;
        }
        
        /// /////////////////////////////////////////////////////////
        /// Get
        /// /////////////////////////////////////////////////////////  

        // CLuster Integrity
        public float AmountIntegrity { get { return  (float)cluster.shards.Count / initShardAmount * 100f; } }
        
    }
}