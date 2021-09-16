using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EasyGameStudio.Jeremy
{
    public class Demo_control : MonoBehaviour
    {

        [Header("index")]
        public int index = 0;

        [Header("gameobjct array")]
        public GameObject[] fore_field_array;


        // Start is called before the first frame update
        void Start()
        {
            this.on_choose_btn(index);
        }

        // Update is called once per frame
        void Update()
        {

        }


        //event
        #region 
        public void on_show_btn()
        {
            this.fore_field_array[this.index].GetComponent<Force_field_control>().show();
        }

        public void on_hide_btn()
        {
            this.fore_field_array[this.index].GetComponent<Force_field_control>().hide();
        }


        public void on_choose_btn(int index)
        {
            for (int i = 0; i < this.fore_field_array.Length; i++)
            {
                this.fore_field_array[i].SetActive(false);
            }

            this.fore_field_array[index].SetActive(true);
            this.index = index;
        }


        #endregion

    }
}