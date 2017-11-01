﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathsPhys
{
    public class MathsUtility
    {

        // Return value in degrees
        public static Vector3 GetEulerAngleFromQuaternion(Quaternion q)
        {
            Vector3 eulerAngles = new Vector3();

            float sqw = q.w * q.w;
            float sqx = q.x * q.x;
            float sqy = q.y * q.y;
            float sqz = q.z * q.z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            float unit = sqx + sqy + sqz + sqw;
            float test = q.x * q.y + q.z * q.w;

            if (test > 0.4999f * unit)                              // 0.4999f OR 0.5f - EPSILON
            {
                // Singularity at north pole
                eulerAngles.x = 0f;                                // Roll
                eulerAngles.y = 180 * 0.5f;                         // Pitch
                eulerAngles.z = 2f * Mathf.Atan2(q.x, q.w) * 180 / Mathf.PI;  // Yaw     
                return eulerAngles;
            }
            else if (test < -0.4999f * unit)                        // -0.4999f OR -0.5f + EPSILON
            {
                // Singularity at south pole
                eulerAngles.x = 0f;                                // Roll
                eulerAngles.y = -180 * 0.5f;                        // Pitch
                eulerAngles.z = -2f * Mathf.Atan2(q.x, q.w) * 180 / Mathf.PI; // Yaw               
                return eulerAngles;
            }
            else
            {
                eulerAngles.x = 180 / Mathf.PI * Mathf.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (sqy + sqz)); // Roll
                eulerAngles.y = 180 / Mathf.PI * Mathf.Asin(2f * (q.x * q.z - q.w * q.y));                             // Pitch           
                eulerAngles.z = 180 / Mathf.PI * Mathf.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (sqz + sqw));     // Yaw  
            }

            return eulerAngles;
        }



        // Input values should be degrees
        public static Quaternion GetQuaternionFromEulerAngle(float roll, float pitch, float yaw)
        {
            //Convert from degree to radian first
            float rollOver2 = roll * 0.5f * Mathf.PI/180;
            float sinRollOver2 = Mathf.Sin(rollOver2);
            float cosRollOver2 = Mathf.Cos(rollOver2);
            float pitchOver2 = pitch * 0.5f * Mathf.PI / 180;
            float sinPitchOver2 = Mathf.Sin(pitchOver2);
            float cosPitchOver2 = Mathf.Cos(pitchOver2);
            float yawOver2 = yaw * 0.5f * Mathf.PI / 180;
            float sinYawOver2 = Mathf.Sin(yawOver2);
            float cosYawOver2 = Mathf.Cos(yawOver2);
            Quaternion result;
            result.x = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
            result.z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.w = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            return result;
        }
    }

}
