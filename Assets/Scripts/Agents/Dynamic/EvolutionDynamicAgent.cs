using AI.FuzzyLogic;
using AI.FuzzyLogic.FuzzyInference;
using AI.FuzzyLogic.Terms;
using AI.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Agents.Dynamic
{
    public class EvolutionDynamicAgent : MonoBehaviour
    {
        public float maxSpeed = 10.0f;

        public float turnSpeed = 3.0f;

        public float slowingDistance = 5.0f;

        public float R = 0.825f;
        public float l = 1.65f;

        public Material bestSkin;

        public void TargetPositionUpdated(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
            this.hasTarget = true;
        }

        public bool IsCrashed()
        {
            return isCrashed;
        }

        void OnTriggerEnter(Collider other)
        {
            isCrashed = true;
        }

        public void UseRegularSkin()
        {
            GetComponent<Renderer>().sharedMaterial = this.originalMaterial;
        }

        public void UseBestSkin()
        {
            GetComponent<Renderer>().sharedMaterial = this.bestSkin;
        }

        private void Awake()
        {
            isCrashed = false;
            this.characterController = this.GetComponent<CharacterController>();
            this.originalMaterial = GetComponent<Renderer>().sharedMaterial;
            this.sensorManager = GetComponent<SensorsManager>();
        }

        private void Start()
        {
            seconds = new TimeSpan(DateTime.Now.Ticks).TotalSeconds;
        }

        public void updateFuzzyParams(FuzzyChromosome chromosome)
        {
            IFuzzyInferenceSystem fuzzyInferenceSystem = new MamdaniFuzzyInference(new Minimum(), new Maximum(), new Minimum(),
                    new Maximum(), new CentroidDefuzzifier());

            fuzzyEngine = new Engine(fuzzyInferenceSystem);

            Term close = chromosome.FuzzyGenes[0].Term;
            Term medium = chromosome.FuzzyGenes[1].Term;
            Term far = chromosome.FuzzyGenes[2].Term;

            LinguisticVariable lls = new LinguisticVariable("lls", 0.0, 15.0);
            lls.AddTerm(close);
            lls.AddTerm(medium);
            lls.AddTerm(far);
            fuzzyEngine.RegisterInputVariable(lls);

            LinguisticVariable ls = new LinguisticVariable("ls", 0.0, 15.0);
            ls.AddTerm(close);
            ls.AddTerm(medium);
            ls.AddTerm(far);
            fuzzyEngine.RegisterInputVariable(ls);

            LinguisticVariable fs = new LinguisticVariable("fs", 0.0, 15.0);
            fs.AddTerm(close);
            fs.AddTerm(medium);
            fs.AddTerm(far);
            fuzzyEngine.RegisterInputVariable(fs);

            LinguisticVariable rs = new LinguisticVariable("rs", 0.0, 15.0);
            rs.AddTerm(close);
            rs.AddTerm(medium);
            rs.AddTerm(far);
            fuzzyEngine.RegisterInputVariable(rs);

            LinguisticVariable rrs = new LinguisticVariable("rrs", 0.0, 15.0);
            rrs.AddTerm(close);
            rrs.AddTerm(medium);
            rrs.AddTerm(far);
            fuzzyEngine.RegisterInputVariable(rrs);


            Term fastNeg = chromosome.FuzzyGenes[3].Term;
            Term mediumNeg = chromosome.FuzzyGenes[4].Term;
            Term slowNeg = chromosome.FuzzyGenes[5].Term;
            Term slowPos = chromosome.FuzzyGenes[6].Term;
            Term mediumPos = chromosome.FuzzyGenes[7].Term;
            Term fastPos = chromosome.FuzzyGenes[8].Term;

            LinguisticVariable speedL = new LinguisticVariable("speedL", -10.0, 10.0);
            speedL.AddTerm(fastNeg);
            speedL.AddTerm(mediumNeg);
            speedL.AddTerm(slowNeg);
            speedL.AddTerm(slowPos);
            speedL.AddTerm(mediumPos);
            speedL.AddTerm(fastPos);
            fuzzyEngine.RegisterOutputVariable(speedL);

            LinguisticVariable speedR = new LinguisticVariable("speedR", -10.0, 10.0);
            speedR.AddTerm(fastNeg);
            speedR.AddTerm(mediumNeg);
            speedR.AddTerm(slowNeg);
            speedR.AddTerm(slowPos);
            speedR.AddTerm(mediumPos);
            speedR.AddTerm(fastPos);
            fuzzyEngine.RegisterOutputVariable(speedR);

            //1
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS close AND rs IS close AND rrs IS close THEN " +
                "speedL IS very fastNeg AND speedR IS very fastNeg");
            //2
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS close AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS mediumNeg AND speedR IS fastNeg");
            //3
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS close AND rs IS far AND rrs IS far THEN " +
                "speedL IS very fastPos AND speedR IS very fastNeg");
            //4
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS medium AND rs IS close AND rrs IS close THEN " +
                "speedL IS mediumNeg AND speedR IS mediumNeg");
            //5
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS medium AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS fastPos AND speedR IS fastNeg");
            //6
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS medium AND rs IS far AND rrs IS far THEN " +
                "speedL IS fastPos AND speedR IS fastNeg");
            //7
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS far AND rs IS close AND rrs IS close THEN " +
                "speedL IS fastNeg AND speedR IS fastNeg");
            //8
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS far AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS fastPos AND speedR IS mediumNeg");
            //9
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS close AND fs IS far AND rs IS far AND rrs IS far THEN " +
                "speedL IS fastPos AND speedR IS mediumPos");
            //10
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS close AND rs IS close AND rrs IS close THEN " +
                "speedL IS fastNeg AND speedR IS mediumNeg");
            //11
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS close AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS very fastNeg AND speedR IS very fastNeg");
            //12
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS close AND rs IS far AND rrs IS far THEN " +
                "speedL IS fastNeg AND speedR IS very fastNeg");
            //13
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS medium AND rs IS close AND rrs IS close THEN " +
                "speedL IS mediumNeg AND speedR IS slowNeg");
            //14
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS medium AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS mediumNeg AND speedR IS mediumNeg");
            //15
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS medium AND rs IS far AND rrs IS far THEN " +
                "speedL IS mediumPos AND speedR IS slowNeg");
            //16
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS far AND rs IS close AND rrs IS close THEN " +
                "speedL IS slowPos AND speedR IS fastPos");
            //17
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS far AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS mediumPos AND speedR IS slowPos");
            //18
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS medium AND fs IS far AND rs IS far AND rrs IS far THEN " +
                "speedL IS fastPos AND speedR IS mediumPos");
            //19
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS close AND rs IS close AND rrs IS close THEN " +
                "speedL IS fastNeg AND speedR IS fastPos");
            //20
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS close AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS fastNeg AND speedR IS fastPos");
            //21
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS close AND rs IS far AND rrs IS far THEN " +
                "speedL IS fastNeg AND speedR IS fastPos");
            //22
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS medium AND rs IS close AND rrs IS close THEN " +
                "speedL IS slowPos AND speedR IS mediumPos");
            //23
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS medium AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS slowPos AND speedR IS mediumPos");
            //24
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS medium AND rs IS far AND rrs IS far THEN " +
                "speedL IS slowPos AND speedR IS mediumPos");
            //25
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS far AND rs IS close AND rrs IS close THEN " +
                "speedL IS slowPos AND speedR IS very fastPos");
            //26
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS far AND rs IS medium AND rrs IS medium THEN " +
                "speedL IS mediumPos AND speedR IS very fastPos");
            //27
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS medium AND fs IS far AND rs IS medium AND rrs IS close THEN " +
                "speedL IS mediumPos AND speedR IS mediumPos");
            //28
            fuzzyEngine.RegisterRule("IF lls IS close AND ls IS far AND fs IS far AND rs IS far AND rrs IS close THEN " +
                "speedL IS very fastPos AND speedR IS very fastPos");
            //29
            fuzzyEngine.RegisterRule("IF lls IS medium AND ls IS far AND fs IS far AND rs IS far AND rrs IS medium THEN " +
                "speedL IS very fastPos AND speedR IS very fastPos");
            //30
            fuzzyEngine.RegisterRule("IF lls IS far AND ls IS far AND fs IS far AND rs IS far AND rrs IS far THEN " +
                "speedL IS very fastPos AND speedR IS very fastPos");
            
            fuzzyEngineInput = new Dictionary<string, double>();
            fuzzyEngineInput.Add("lls", 0.0);
            fuzzyEngineInput.Add("ls", 0.0);
            fuzzyEngineInput.Add("fs", 0.0);
            fuzzyEngineInput.Add("rs", 0.0);
            fuzzyEngineInput.Add("rrs", 0.0);

            isInit = true;
        }

        private void Update()
        {
            double newSeconds = new TimeSpan(DateTime.Now.Ticks).TotalSeconds;
            if (newSeconds - seconds > 10)
            {
                if (transform.position.x - previousPosition < 10)
                {
                    isCrashed = true;
                    //Debug.Log("Kill slow agent");
                }

                seconds = newSeconds;
                previousPosition = transform.position.x;
            }
            if (transform.position.z > zBoundMax + 1 || transform.position.z < zBoundMin - 1)
            {
                isCrashed = true;
                Debug.Log(transform.position.z);
                Debug.Log("Kill lost agent");
            }
            if (isInit)
            {
                if (hasTarget)
                {
                    fuzzyEngineInput["lls"] = sensorManager.SensorsOutput[0];
                    fuzzyEngineInput["ls"] = sensorManager.SensorsOutput[1];
                    fuzzyEngineInput["fs"] = sensorManager.SensorsOutput[2];
                    fuzzyEngineInput["rs"] = sensorManager.SensorsOutput[3];
                    fuzzyEngineInput["rrs"] = sensorManager.SensorsOutput[4];
                    fuzzyEngineOutput = fuzzyEngine.Process(fuzzyEngineInput);
                    if (!fuzzyEngineOutput.ContainsKey("speedL") || !fuzzyEngineOutput.ContainsKey("speedR") ||
                        double.IsNaN(fuzzyEngineOutput["speedL"]) || double.IsNaN(fuzzyEngineOutput["speedR"]))
                    {
                        isCrashed = true;
                    }
                    else
                    {
                        Vector2 speed = GetSpeed();
                        Vector2 target = new Vector2(targetPosition.x, targetPosition.z);
                        Vector2 right = new Vector2(1, 0);
                        float currentRotation = transform.eulerAngles.y;
                        float angle = Vector2.SignedAngle(right, target / target.magnitude);
                        float alpha = (90 - currentRotation - angle) * Time.deltaTime * turnSpeed;
                        //скорости вращения для поворота
                        float omegaL = 2 * (float)Math.Tan(alpha * Math.PI / 180) / R;
                        float omegaR = -omegaL;

                        float deltaV1 = speed.x * Time.deltaTime;
                        float deltaV2 = speed.y * Time.deltaTime;

                        omegaL = omegaL + deltaV1;
                        omegaR = omegaR + deltaV2;

                        float v1 = R * omegaL;
                        float v2 = R * omegaR;
                        float phi = -currentRotation;
                        float xNew = transform.position.x - 0.5f * (v2 + v1) * (float)Math.Sin(phi * Math.PI / 180);
                        float yNew = transform.position.z + 0.5f * (v2 + v1) * (float)Math.Cos(phi * Math.PI / 180);
                        float phiDif = (float)(Math.Atan((v2 - v1) / (2 * l)) * 180 / Math.PI);
                        float phiNew = phi + phiDif;
                        float newRotation = -phiNew;

                        transform.rotation = Quaternion.AngleAxis(newRotation, Vector3.up);
                        transform.position = new Vector3(xNew, transform.position.y, yNew);
                    }
                }
            }
        }

        private Vector2 GetSpeed()
        {
            float speedL = (float)fuzzyEngineOutput["speedL"];
            float speedR = (float)fuzzyEngineOutput["speedR"];
            return new Vector2(speedL, speedR);
        }

        public void UpdateBounds(double min, double max)
        {
            zBoundMin = min;
            zBoundMax = max;
        }

        private Vector3 targetPosition;

        private bool hasTarget = false;

        private CharacterController characterController;

        private Material originalMaterial;

        private SensorsManager sensorManager;

        private Engine fuzzyEngine;

        private Dictionary<string, double> fuzzyEngineInput;

        private Dictionary<string, double> fuzzyEngineOutput;

        private double zBoundMin = 0;
        private double zBoundMax = 0;

        private bool isCrashed;

        private bool isInit = false;

        private double seconds = 0;
        private double previousPosition = 0;
    }
}
