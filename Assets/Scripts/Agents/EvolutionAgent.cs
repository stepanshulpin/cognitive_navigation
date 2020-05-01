using System;
using System.Collections.Generic;
using AI.FuzzyLogic;
using AI.FuzzyLogic.FuzzyInference;
using AI.FuzzyLogic.Terms;
using AI.GeneticAlgorithm;
using UnityEngine;

public class EvolutionAgent : MonoBehaviour
{
    public float maxSpeed = 10.0f;

    public float turnSpeed = 15.0f;

    public float slowingDistance = 5.0f;

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
        Term far = chromosome.FuzzyGenes[1].Term;

        LinguisticVariable lls = new LinguisticVariable("lls", 0.0, 15.0);
        lls.AddTerm(close);
        lls.AddTerm(far);
        fuzzyEngine.RegisterInputVariable(lls);

        LinguisticVariable ls = new LinguisticVariable("ls", 0.0, 15.0);
        ls.AddTerm(close);
        ls.AddTerm(far);
        fuzzyEngine.RegisterInputVariable(ls);

        LinguisticVariable fs = new LinguisticVariable("fs", 0.0, 15.0);
        fs.AddTerm(close);
        fs.AddTerm(far);
        fuzzyEngine.RegisterInputVariable(fs);

        LinguisticVariable rs = new LinguisticVariable("rs", 0.0, 15.0);
        rs.AddTerm(close);
        rs.AddTerm(far);
        fuzzyEngine.RegisterInputVariable(rs);

        LinguisticVariable rrs = new LinguisticVariable("rrs", 0.0, 15.0);
        rrs.AddTerm(close);
        rrs.AddTerm(far);
        fuzzyEngine.RegisterInputVariable(rrs);


        Term slow = chromosome.FuzzyGenes[2].Term;
        Term medium = chromosome.FuzzyGenes[3].Term;
        Term fast = chromosome.FuzzyGenes[4].Term;

        LinguisticVariable speed = new LinguisticVariable("speed", 0.0, 10.0);
        speed.AddTerm(slow);
        speed.AddTerm(medium);
        speed.AddTerm(fast);
        fuzzyEngine.RegisterOutputVariable(speed);

        Term left = chromosome.FuzzyGenes[5].Term;
        Term forward = chromosome.FuzzyGenes[6].Term;
        Term right = chromosome.FuzzyGenes[7].Term;

        LinguisticVariable direction = new LinguisticVariable("direction", -45.0, 45.0);
        direction.AddTerm(left);
        direction.AddTerm(forward);
        direction.AddTerm(right);
        fuzzyEngine.RegisterOutputVariable(direction);

        //fuzzyEngine.RegisterRule("IF lls IS far THEN speed IS fast AND direction IS forward");
        fuzzyEngine.RegisterRule("IF lls IS close THEN speed IS medium AND direction IS right");

        //fuzzyEngine.RegisterRule("IF ls IS far THEN speed IS fast AND direction IS forward");
        fuzzyEngine.RegisterRule("IF ls IS close THEN speed IS medium AND direction IS right");

        fuzzyEngine.RegisterRule("IF fs IS far AND ls IS far AND rs IS far THEN speed IS fast AND direction IS forward");
        fuzzyEngine.RegisterRule("IF fs IS close AND rs IS far THEN speed IS medium AND direction IS right");
        fuzzyEngine.RegisterRule("IF fs IS close AND ls IS far THEN speed IS medium AND direction IS left");
        fuzzyEngine.RegisterRule("IF fs IS very close AND rs IS far THEN speed IS slow AND direction IS right");
        fuzzyEngine.RegisterRule("IF fs IS very close AND ls IS far THEN speed IS slow AND direction IS left");
        fuzzyEngine.RegisterRule("IF fs IS close AND rs IS close AND ls IS close THEN speed IS slow AND direction IS very right");

        //fuzzyEngine.RegisterRule("IF rs IS far THEN speed IS fast AND direction IS forward");
        fuzzyEngine.RegisterRule("IF rs IS close THEN speed IS medium AND direction IS left");

        //fuzzyEngine.RegisterRule("IF rrs IS far THEN speed IS fast AND direction IS forward");
        fuzzyEngine.RegisterRule("IF rrs IS close THEN speed IS medium AND direction IS left");

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
                Debug.Log("Kill slow agent");
            }

            seconds = newSeconds;
            previousPosition = transform.position.x;
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
                if (!fuzzyEngineOutput.ContainsKey("direction") || !fuzzyEngineOutput.ContainsKey("speed") ||
                    double.IsNaN(fuzzyEngineOutput["direction"]) || double.IsNaN(fuzzyEngineOutput["speed"]))
                {
                    isCrashed = true;
                }
                else
                {
                    Vector3 desiredDirection = GetMovementDirection(targetPosition);
                    Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
                    float movementSpeed = GetMovementSpeed(targetPosition);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                    if (Mathf.Approximately(characterController.velocity.magnitude, 0.0f))
                    {
                        if (Quaternion.Angle(transform.rotation, targetRotation) < MIN_ANGLE_THRESHOLD)
                        {
                            characterController.Move(transform.forward * movementSpeed * Time.deltaTime);
                        }
                    }
                    else
                    {
                        characterController.Move(transform.forward * movementSpeed * Time.deltaTime);
                    }

                }
            }
        }
    }

    private Vector3 GetMovementDirection(Vector3 targetPosition)
    {
        Vector3 desiredDirection = targetPosition - this.transform.position;
        float rotationAngle = (float)fuzzyEngineOutput["direction"];
        desiredDirection.y = 0.0f;
        desiredDirection = Quaternion.AngleAxis(rotationAngle, Vector3.up) * desiredDirection;
        return desiredDirection;
    }

    private float GetMovementSpeed(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - this.transform.position;
        direction.y = 0.0f;
        float controllerSpeed = (float)fuzzyEngineOutput["speed"];
        return Mathf.Min(this.maxSpeed, Math.Min(direction.magnitude / this.slowingDistance * this.maxSpeed, controllerSpeed));
    }

    private Vector3 targetPosition;

    private bool hasTarget = false;

    private CharacterController characterController;

    private Material originalMaterial;

    private SensorsManager sensorManager;

    private static readonly float MIN_ANGLE_THRESHOLD = 10.0f;

    private Engine fuzzyEngine;

    private Dictionary<string, double> fuzzyEngineInput;

    private Dictionary<string, double> fuzzyEngineOutput;

    private bool isCrashed;

    private bool isInit = false;

    private double seconds = 0;
    private double previousPosition = 0;

    private void junk()
    {
        float rotationAngle = 0;
        if (this.sensorManager.SensorsOutput[0] < 7)
        {
            if (this.sensorManager.SensorsOutput[0] < 5)
            {
                rotationAngle += 15.0f;
            }
            else
            {
                rotationAngle += 5.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[1] < 7)
        {
            if (this.sensorManager.SensorsOutput[1] < 5)
            {
                rotationAngle += 25.0f;
            }
            else
            {
                rotationAngle += 15.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[2] < 7)
        {
            if (this.sensorManager.SensorsOutput[2] < 5)
            {
                rotationAngle += 45.0f;
            }
            else
            {
                rotationAngle += 15.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[3] < 7)
        {
            if (this.sensorManager.SensorsOutput[3] < 5)
            {
                rotationAngle -= 25.0f;
            }
            else
            {
                rotationAngle -= 15.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[4] < 7)
        {
            if (this.sensorManager.SensorsOutput[4] < 5)
            {
                rotationAngle -= 15.0f;
            }
            else
            {
                rotationAngle -= 5.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[3] < 7)
        {
            rotationAngle -= 15.0f;
        }
        if (this.sensorManager.SensorsOutput[4] < 7)
        {
            rotationAngle -= 15.0f;
        }

    }
}
