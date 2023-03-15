using UnityEngine;
using System.Collections;

namespace Highlighters
{
    public class Camera_Controller : MonoBehaviour
    {
        public float Normal_Speed = 25.0f; //Normal movement speed

        public float Shift_Speed = 54.0f; //multiplies movement speed by how long shift is held down.

        public float Speed_Cap = 54.0f; //Max cap for speed when shift is held down

        public float Camera_Sensitivity = 0.6f; //How sensitive it with mouse

        private Vector3 Mouse_Location = new Vector3(255, 255, 255); //Mouse location on screen during play (Set to near the middle of the screen)

        private float Total_Speed = 1.0f; //Total speed variable for shift

        public bool freeze = false;
        public bool freezeMouse = true;

        void Update()
        {

            if (Input.GetKey(KeyCode.Escape))
            {
                freeze = !freeze;
            }
            if (freeze) return;

            if (Input.GetKey(KeyCode.M))
            {
                freezeMouse = !freezeMouse;
            }
            if (!freezeMouse)
            {

                //Camera angles based on mouse position
                Mouse_Location = Input.mousePosition - Mouse_Location;

                Mouse_Location = new Vector3(-Mouse_Location.y * Camera_Sensitivity, Mouse_Location.x * Camera_Sensitivity, 0);

                Mouse_Location = new Vector3(transform.eulerAngles.x + Mouse_Location.x, transform.eulerAngles.y + Mouse_Location.y, 0);

                transform.eulerAngles = Mouse_Location;

                Mouse_Location = Input.mousePosition;

            }





            //Keyboard controls

            Vector3 Cam = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift))
            {


                Total_Speed += Time.deltaTime;

                Cam = Cam * Total_Speed * Shift_Speed;

                Cam.x = Mathf.Clamp(Cam.x, -Speed_Cap, Speed_Cap);

                Cam.y = Mathf.Clamp(Cam.y, -Speed_Cap, Speed_Cap);

                Cam.z = Mathf.Clamp(Cam.z, -Speed_Cap, Speed_Cap);



            }
            else
            {


                Total_Speed = Mathf.Clamp(Total_Speed * 0.5f, 1f, 1000f);

                Cam = Cam * Normal_Speed;


            }

            Cam = Cam * Time.deltaTime;

            Vector3 newPosition = transform.position;

            if (Input.GetKey(KeyCode.Space))
            {


                //If the player wants to move on X and Z axis only by pressing space (good for re-adjusting angle shots)
                transform.Translate(Cam);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;


            }
            else
            {


                transform.Translate(Cam);


            }


        }

        private Vector3 GetBaseInput()
        {


            Vector3 Camera_Velocity = new Vector3();

            float HorizontalInput = Input.GetAxis("Horizontal"); //Input for horizontal movement

            float VerticalInput = Input.GetAxis("Vertical"); //Input for Vertical movement



            Camera_Velocity += new Vector3(HorizontalInput, 0, 0);

            Camera_Velocity += new Vector3(0, 0, VerticalInput);

            return Camera_Velocity;


        }
    }
}