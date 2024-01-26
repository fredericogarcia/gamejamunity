using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject KazooVisualAsset;
    [SerializeField] GameObject KazooProjectile;
    [SerializeField] Transform Firepoint;
    public Rigidbody2D rb;
    public float JumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask WhatIsGround;
    //-------------------------------------------
    [SerializeField] float TimeBetweenShot;
    float ShotTimer;
    bool CanDoubleJump;
    //-------------------------------------------
    public float Health = 100;
    //-------------------------------------------
    public Animator anim;
    [SerializeField] float LongIdleTimer;
    float TimeForLongIdle = 5;
    [SerializeField] GameObject FartAnim;
    SpriteRenderer spr;
    [SerializeField]GameObject FartForFLip;
    [SerializeField] GameObject FartWordFLip;
    //-------------------------------------------
    private Vector2 AimDir;
    public bool UsingController;
    //-------------------------------------------
    public AudioClip[] FartAudio;
    public AudioClip HitgroundClip;
    public AudioClip[] KazooAudio;
    public AudioClip LetterPickup;
    public AudioClip[] DeathSounds;
    public AudioClip BleepedDeath;
    AudioSource Asource;
    bool JustLanded;
    //-------------------------------------------
    [SerializeField] ParticleSystem DustParticles;
    bool Disable = false;
    //-------------------------------------------
    public GameObject deathScreen;
    public GameObject RestartButton;
    public UIControls UIC;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        Asource = GetComponent<AudioSource>();
        UIC = FindObjectOfType<UIControls>();
    }

    // Update is called once per frame
    void Update()
    {
       // UsingController = UIC.controller;
        if (!Disable)
        {
            if (!UsingController)
            {
                var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                KazooVisualAsset.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            ShotTimer -= Time.deltaTime;


            if (Input.GetButtonDown("Fire1") && IsGrounded() && ShotTimer <= 0)
            {
                FireKazoo();
                var RandomKazoo = Random.Range(1, KazooAudio.Length);
                Asource.Stop();
                Asource.clip = KazooAudio[RandomKazoo];
                Asource.Play();
            }
            else if (Input.GetButtonDown("Fire1") && !IsGrounded() && CanDoubleJump)
            {
                KazooVisualAsset.SetActive(false);
                CanDoubleJump = false;
                FireKazoo();
                anim.SetTrigger("FartJump");
                FartAnim.SetActive(true);
                var RandomFart = Random.Range(1, FartAudio.Length);
                Asource.Stop();
                Asource.clip = FartAudio[RandomFart];
                Asource.Play();
            }

            if (IsGrounded())
            {
                KazooVisualAsset.SetActive(true);
                CanDoubleJump = true;
                anim.ResetTrigger("FartJump");
                FartAnim.SetActive(false);
                if (!JustLanded)
                {
                    JustLanded = true;
                    DustParticles.Play();
                    Asource.Stop();
                    Asource.clip = HitgroundClip;
                    Asource.Play();
                }
            }
            else
            {
                JustLanded = false;
            }

            anim.SetBool("Grounded", IsGrounded());
            anim.SetFloat("YVelocity", rb.velocity.y);
            LongIdleTimer -= Time.deltaTime;
            if (LongIdleTimer <= 0)
            {
                KazooVisualAsset.SetActive(false);
                anim.SetBool("LongIdle", true);
            }

            if (LongIdleTimer <= 0 && (Input.anyKey || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
            {
                LongIdleTimer = TimeForLongIdle;
                anim.SetBool("LongIdle", false);
                KazooVisualAsset.SetActive(true);
            }
            else if (Input.anyKey || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                LongIdleTimer = TimeForLongIdle;
            }

            if (rb.velocity.x > 0)
            {
                spr.flipX = false;
                FartForFLip.transform.localScale = new Vector3(1, 1, 1);
                FartWordFLip.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (rb.velocity.x < 0)
            {
                spr.flipX = true;
                FartForFLip.transform.localScale = new Vector3(-1, 1, 1);
                FartWordFLip.GetComponent<SpriteRenderer>().flipX = true;
            }

            //Controller Movement=================================================
            AimDir.y = Input.GetAxisRaw("Horizontal");
            AimDir.x = -Input.GetAxisRaw("Vertical");

            if (UsingController)
            {
                KazooVisualAsset.transform.rotation = Quaternion.LookRotation(Vector3.forward, AimDir);
            }
        }
    }
    public void FireKazoo()
    {
       ShotTimer = TimeBetweenShot;
       var projectile = Instantiate(KazooProjectile, Firepoint.position, Firepoint.rotation);
       projectile.transform.SetParent(null);
    }
    [SerializeField] public bool IsGrounded()
    {
       return Physics2D.OverlapCircle(groundCheck.position, 1, WhatIsGround);
    }
    public void TakeDamage(float Damage)
    {
        Health -= Damage;
        if(Health <= 0)
        {
            var RandomDeath = Random.Range(1, DeathSounds.Length);
            Asource.Stop();
            if(PlayerPrefs.GetInt("18?") == 0)
            {
                Asource.clip = DeathSounds[RandomDeath];
            }
            else
            {
                Asource.clip = BleepedDeath;
            }
            Asource.Play();
            spr.enabled = false;
            Disable = true;
            deathScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(RestartButton);
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 1);
    }
}
