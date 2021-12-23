using UnityEngine;
using System.Collections;

public class BreakableV2 : MonoBehaviour {

    public bool UseCollisionForce;
    public float CollisionForce;
    public bool UseHp;
    public float MaxHp;
    public AudioClip[] CollisionSounds;
    public bool UseThisObject;
    public Transform[] CollisionPrefabs;
    public float CollisionEffectDuration;

    public AudioClip[] BreakingSounds;
    public Transform[] BrokenPrefabs;
    public float BreakDelay;
    public Transform[] AfterDelayBrokenPrefabs;
    public bool ApplyExplosionForce;
    public float ExplosionForce;
    public float ExplosionRadius;

    public bool CanReset = true;
    public bool UseAutoDestruct;
    public float AutoDestructTimer;

    public float currentHp;
    public bool broken;
    private GameObject otherCol;
    private Vector3 otherOrgVel;
    private Vector3 otherAddVelocity;
    private AudioSource CollisionAudio;
    private AudioSource BreakAudio;
    private bool delayedSpawned;
    private Vector3 orgPos;
    private Quaternion orgRot;
    private bool orgKinematic;

    private float collEffTimer;
    public float delayTimer;
    public float destructionTimer;

   
    
    private Transform PrespawnedBroken;
    private Transform PrespawnedBrokenDelay;
    private Transform[] PrespawnedCollisionObjects;

    void Start () {

        if (GetComponent<Rigidbody>())
        {
            orgKinematic = GetComponent<Rigidbody>().isKinematic;
        }
        else {
            Debug.LogWarning("Destructible: object " + gameObject.name + " does not have a rigidbody, disabling object");
        }
        orgPos = transform.position;
        orgRot = transform.rotation;

        broken = false;
        delayedSpawned = false;
        currentHp = MaxHp;
        delayTimer = BreakDelay;
        destructionTimer = AutoDestructTimer;

        if (CollisionSounds.Length > 0) {
            CollisionAudio = gameObject.AddComponent<AudioSource>();
        }

        if (BreakingSounds.Length > 0) {
            BreakAudio = gameObject.AddComponent<AudioSource>();
        }

        if (UseThisObject == false)
        {
            if (BrokenPrefabs.Length > 0)
            {
                PrespawnedBroken = (Transform)Instantiate(BrokenPrefabs[Random.Range(0, BrokenPrefabs.Length)], transform.position, transform.rotation);
                PrespawnedBroken.gameObject.SetActive(false);
                PrespawnedBroken.hideFlags = HideFlags.HideInHierarchy;
                PrespawnedBroken.gameObject.name = "Prespawned destructible object";
            }
            else {
                Debug.LogWarning("Destructible: object " + gameObject.name + " has no BrokenPrefabs assigned, disabling object");
                gameObject.SetActive(false);
            }
        }

        if (BreakDelay > 0)
        {
            if (AfterDelayBrokenPrefabs.Length > 0)
            {
                PrespawnedBrokenDelay = (Transform)Instantiate(AfterDelayBrokenPrefabs[Random.Range(0, AfterDelayBrokenPrefabs.Length)], transform.position, transform.rotation);
                PrespawnedBrokenDelay.gameObject.SetActive(false);
                PrespawnedBrokenDelay.hideFlags = HideFlags.HideInHierarchy;
                PrespawnedBrokenDelay.gameObject.name = "Prespawned destructible object";
            }
            else
            {
                Debug.LogWarning("Destructible: object " + gameObject.name + " has no AfterDelayBrokenPrefabs assigned, disabling object");
                gameObject.SetActive(false);
            }
        }
        PrespawnedCollisionObjects = new Transform[0];
        if (CollisionPrefabs.Length > 0)
        {
            PrespawnedCollisionObjects = new Transform[CollisionPrefabs.Length];
            for (int i = 0; i < CollisionPrefabs.Length; i++)
            {
                PrespawnedCollisionObjects[i] = (Transform)Instantiate(CollisionPrefabs[i], transform.position, transform.rotation);
                PrespawnedCollisionObjects[i].gameObject.SetActive(false);
                PrespawnedCollisionObjects[i].hideFlags = HideFlags.HideInHierarchy;
                PrespawnedCollisionObjects[i].gameObject.name = "Prespawned destructible object";
            }
        }
    }
	
	void Update () {

        if (PrespawnedCollisionObjects.Length > 0 && PrespawnedCollisionObjects[0] != null)
        {
            if (collEffTimer > 0)
            {
                collEffTimer -= Time.deltaTime;
            }
            else
            {
                foreach (Transform prspwndCollEff in PrespawnedCollisionObjects)
                {
                    prspwndCollEff.gameObject.SetActive(false);
                }
            }
        }

        if (BreakDelay > 0 && delayedSpawned == false && broken == true)
        {
            if (delayTimer > 0)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                PrespawnedBrokenDelay.position = transform.position;
                PrespawnedBrokenDelay.gameObject.SetActive(true);
                delayedSpawned = true;
            }
        }
        if (UseAutoDestruct == true && broken == true) {
            if (destructionTimer > 0)
            {
                destructionTimer -= Time.deltaTime;
            }
            else {
                if (PrespawnedBroken != null)
                {
                    Destroy(PrespawnedBroken.gameObject);
                }
                if (PrespawnedBrokenDelay != null)
                {
                    Destroy(PrespawnedBrokenDelay.gameObject);
                }
                foreach (Transform prspwndCollEff in PrespawnedCollisionObjects)
                {
                    if (prspwndCollEff != null)
                    {
                        Destroy(prspwndCollEff.gameObject);
                    }
                }
                if (CanReset == false)
                {
                    Destroy(gameObject);
                }
                else {
                    if (GetComponent<Collider>())
                    {
                        GetComponent<Collider>().enabled = false;
                    }
                    if (GetComponent<MeshRenderer>())
                    {
                        GetComponent<MeshRenderer>().enabled = false;
                    }
                    GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        otherOrgVel = collision.relativeVelocity - gameObject.GetComponent<Rigidbody>().velocity;
        otherCol = collision.gameObject;

        if (broken == false)
        {
            if (otherCol.GetComponent<Rigidbody>())
            {
                //CollisionForce
                if (UseCollisionForce == true)
                {
                    if (collision.relativeVelocity.magnitude * otherCol.GetComponent<Rigidbody>().mass >= CollisionForce)
                    {
                        //Compensate enegy loss
                        if (GetComponent<Rigidbody>().isKinematic == true)
                        {
                            float kineticEnegyUsed = ((collision.relativeVelocity.magnitude * otherCol.GetComponent<Rigidbody>().mass - CollisionForce) / otherCol.GetComponent<Rigidbody>().mass) / otherOrgVel.magnitude;
                            otherAddVelocity = otherOrgVel * kineticEnegyUsed;
                            otherCol.GetComponent<Rigidbody>().velocity = otherAddVelocity;
                        }
                        BrakeObject();
                    }
                }
                if (UseHp == true)
                {
                    currentHp -= collision.relativeVelocity.magnitude * otherCol.GetComponent<Rigidbody>().mass;
                    if (currentHp <= 0)
                    {
                        //Compensate enegy loss
                        if (GetComponent<Rigidbody>().isKinematic == true)
                        {
                            float kineticEnegyUsed = ((collision.relativeVelocity.magnitude * otherCol.GetComponent<Rigidbody>().mass - CollisionForce) / otherCol.GetComponent<Rigidbody>().mass) / otherOrgVel.magnitude;
                            otherAddVelocity = otherOrgVel * kineticEnegyUsed;
                            otherCol.GetComponent<Rigidbody>().velocity = otherAddVelocity;
                        }
                        BrakeObject();
                    }
                }
            }
        }

        //Collision effects
        if (PrespawnedCollisionObjects.Length > 0) {
            Transform curEffect;
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.otherCollider.GetComponent<Rigidbody>()) {
                    curEffect = PrespawnedCollisionObjects[Random.Range(0, (PrespawnedCollisionObjects.Length))];
                    curEffect.position = contact.point;
                    collEffTimer = CollisionEffectDuration;
                    curEffect.gameObject.SetActive(true);
                    break;
                }
            }
        }

        //Collision sounds
        if (CollisionAudio != null && CollisionSounds.Length > 0)
        {
            if (BreakAudio == null)
            {
                CollisionAudio.clip = CollisionSounds[Random.Range(0, CollisionSounds.Length)];
                CollisionAudio.Play();
            }
            else {
                if (!BreakAudio.isPlaying) {
                    CollisionAudio.clip = CollisionSounds[Random.Range(0, CollisionSounds.Length)];
                    CollisionAudio.Play();
                }
            }
        }
    }

    void BrakeObject()
    {
        //Disable objects
        if (UseThisObject == true)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            if (GetComponent<Collider>())
            {
                GetComponent<Collider>().enabled = false;
            }
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            GetComponent<Rigidbody>().isKinematic = true;
            PrespawnedBroken.position = transform.position;
            PrespawnedBroken.rotation = transform.rotation;
            PrespawnedBroken.gameObject.SetActive(true);
        }
        //Breaking sound
        if (BreakAudio != null && BreakingSounds.Length > 0)
        {
            if (CollisionAudio != null)
            {
                CollisionAudio.Stop();
            }
            BreakAudio.clip = BreakingSounds[Random.Range(0, BreakingSounds.Length)];
            BreakAudio.Play();
        }
        //Explosion
        if (ApplyExplosionForce == true)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, ExplosionRadius);
            foreach (Collider hit in cols)
            {
                if (hit.GetComponent<Rigidbody>() != null)
                {
                    hit.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                }
            }
        }

        broken = true;
    }

    public void Reset() {

        if (CanReset == true)
        {
            if (PrespawnedBroken != null)
            {
                Destroy(PrespawnedBroken.gameObject);
            }
            if (PrespawnedBrokenDelay != null)
            {
                Destroy(PrespawnedBrokenDelay.gameObject);
            }
            foreach (Transform prspwndCollEff in PrespawnedCollisionObjects)
            {
                if (prspwndCollEff != null)
                {
                    Destroy(prspwndCollEff.gameObject);
                }
            }
            if (GetComponent<Collider>())
            {
                GetComponent<Collider>().enabled = true;
            }
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = orgPos;
            transform.rotation = orgRot;
            GetComponent<Rigidbody>().isKinematic = orgKinematic;

            broken = false;
            delayedSpawned = false;
            currentHp = MaxHp;
            delayTimer = BreakDelay;
            destructionTimer = AutoDestructTimer;

            if (UseThisObject == false)
            {
                if (BrokenPrefabs.Length > 0)
                {
                    PrespawnedBroken = (Transform)Instantiate(BrokenPrefabs[Random.Range(0, BrokenPrefabs.Length)], transform.position, transform.rotation);
                    PrespawnedBroken.gameObject.SetActive(false);
                    PrespawnedBroken.hideFlags = HideFlags.HideInHierarchy;
                    PrespawnedBroken.gameObject.name = "Prespawned destructible object";
                }
                else
                {
                    Debug.LogWarning("Destructible: object " + gameObject.name + " has no BrokenPrefabs assigned, disabling object");
                    gameObject.SetActive(false);
                }
            }

            if (BreakDelay > 0)
            {
                if (AfterDelayBrokenPrefabs.Length > 0)
                {
                    PrespawnedBrokenDelay = (Transform)Instantiate(AfterDelayBrokenPrefabs[Random.Range(0, AfterDelayBrokenPrefabs.Length)], transform.position, transform.rotation);
                    PrespawnedBrokenDelay.gameObject.SetActive(false);
                    PrespawnedBrokenDelay.hideFlags = HideFlags.HideInHierarchy;
                    PrespawnedBrokenDelay.gameObject.name = "Prespawned destructible object";
                }
                else
                {
                    Debug.LogWarning("Destructible: object " + gameObject.name + " has no AfterDelayBrokenPrefabs assigned, disabling object");
                    gameObject.SetActive(false);
                }
            }
            PrespawnedCollisionObjects = new Transform[0];
            if (CollisionPrefabs.Length > 0)
            {
                PrespawnedCollisionObjects = new Transform[CollisionPrefabs.Length];
                for (int i = 0; i < CollisionPrefabs.Length; i++)
                {
                    PrespawnedCollisionObjects[i] = (Transform)Instantiate(CollisionPrefabs[i], transform.position, transform.rotation);
                    PrespawnedCollisionObjects[i].gameObject.SetActive(false);
                    PrespawnedCollisionObjects[i].hideFlags = HideFlags.HideInHierarchy;
                    PrespawnedCollisionObjects[i].gameObject.name = "Prespawned destructible object";
                }
            }
        }
    }
}
