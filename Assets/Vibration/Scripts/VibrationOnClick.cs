using System.Collections;
using Things;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vibration
{
    [RequireComponent(typeof(MeshRenderer))]
    public class VibrationOnClick : MaterialPropertiesBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private IProperty<float> strengthProperty;
        private IProperty<float> distanceStrengthProperty;
        private IProperty<Vector4> axisProperty;

        [Header("Static")]
        public Vector3 Origin = new Vector3(0,0,0);
        public AudioClip Sound;
        [Header("Parameters")]
        public float Strength = 0.1f;
        public float DistanceStrength = 0.1f;
        public float StrengthDecreaseSpeed = 0.5f;

        public bool Randomize = false;

        protected override void PrepareProperties(MaterialProperties properties)
        {
            if (Randomize)
            {
                Strength = Random.Range(0, Strength);
                DistanceStrength = Random.Range(0, DistanceStrength);
                StrengthDecreaseSpeed = Random.Range(0, StrengthDecreaseSpeed);
            }

            strengthProperty = properties.GetFloatProperty("_Strength");
            distanceStrengthProperty = properties.GetFloatProperty("_DistanceStrength");
            axisProperty = properties.GetVectorProperty("_Axis");
            axisProperty.Value = Vector3.forward;
            properties.GetVectorProperty("_Origin").Value = Origin;
        }

        private IEnumerator UpdateCycle()
        {
            while (ApplyProperties())
            {
                var deltaTime = Time.deltaTime;
                strengthProperty.Value = Mathf.Clamp(strengthProperty.Value - StrengthDecreaseSpeed * deltaTime, 0,
                    float.MaxValue);
                distanceStrengthProperty.Value = Mathf.Clamp(distanceStrengthProperty.Value - StrengthDecreaseSpeed * deltaTime, 0,
                    float.MaxValue);
                yield return null;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            strengthProperty.Value = Strength;
            distanceStrengthProperty.Value = DistanceStrength;
            var randomC = Random.insideUnitCircle;
            axisProperty.Value = new Vector4(randomC.x, 0, randomC.y);
            AudioSource.PlayClipAtPoint(Sound, transform.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.StopAllCoroutines();
            StartCoroutine(UpdateCycle());
        }
    }
}
