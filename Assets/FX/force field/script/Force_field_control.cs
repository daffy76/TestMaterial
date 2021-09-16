using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGameStudio.Jeremy
{
    public class Force_field_control: MonoBehaviour
    {

        [Header("show speed")]
        [Range(0, 1f)]
        public float speed_show=0.6f;

        [Header("hide speed")]
        [Range(0, 1f)]
        public float hide_show=0.6f;

        [Header("audio source, show audio, hide audio")]
        public AudioSource audio_source;
        public AudioClip audio_clip_show;
        public AudioClip audio__clip_hide;


        private Material _material;
        private bool is_showing;
        private bool is_hiding;
        private float threshold = 0;

        // Start is called before the first frame update
        void Start()
        {
            if (this.GetComponent<MeshRenderer>() == null)
            {
                this._material = this.GetComponent<SkinnedMeshRenderer>().material;
            }
            else
            {
                this._material = this.GetComponent<MeshRenderer>().material;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (this.is_showing)
            {
                //this.threshold = Mathf.Lerp(this.threshold, 0.51f, Time.deltaTime * this.speed_show);
                this.threshold += Time.deltaTime * this.speed_show;

                if (this.threshold >= 0.6)
                {
                    this.threshold = 0.6f;

                    this.is_showing = false;
                }

                this._material.SetFloat("float_animation_threshold", this.threshold);
            }

            if (this.is_hiding)
            {
                //this.threshold = Mathf.Lerp(this.threshold, -0.51f, Time.deltaTime * this.speed_show);
                this.threshold-= Time.deltaTime * this.speed_show;

                if (this.threshold <=-0.6f)
                {
                    this.threshold = -0.6f;

                    this.is_hiding = false;
                }
                this._material.SetFloat("float_animation_threshold", this.threshold);
            }
        }

        public void show()
        {
            this.is_hiding = false;

            this.threshold = -0.6f;

            this._material.SetFloat("float_animation_threshold", this.threshold);

            this.is_showing = true;

            StartCoroutine(this.play_start_audio());
        }

        public void hide()
        {
            this.is_showing = false;

            this.threshold = 0.6f;

            this._material.SetFloat("float_animation_threshold", this.threshold);

            this.is_hiding = true;

            this.audio_source.clip = this.audio__clip_hide;
            this.audio_source.Play();
        }

        public IEnumerator play_start_audio()
        {
            yield return new WaitForSeconds(0.3f);

            this.audio_source.clip = this.audio_clip_show;
            this.audio_source.Play();
        }
    }
}