using System;
using System.Collections.Generic;
using AI.FuzzyLogic;
using AI.FuzzyLogic.FuzzyInference;
using AI.FuzzyLogic.Terms;
using UnityEngine;

public class InfinityCourseAgent : MonoBehaviour {
    public float maxSpeed = 5.0f;

    public float turnSpeed = 15.0f;

    public float slowingDistance = 5.0f;

    public Material bestSkin;

    public void TargetPositionUpdated(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
        this.hasTarget = true;
    }

    public void UseRegularSkin() {
        GetComponent<Renderer>().sharedMaterial = this.originalMaterial;
    }

    public void UseBestSkin() {
        GetComponent<Renderer>().sharedMaterial = this.bestSkin;
    }

    private void Awake() {
        this.characterController = this.GetComponent<CharacterController>();
        this.originalMaterial = GetComponent<Renderer>().sharedMaterial;
        this.sensorManager = GetComponent<SensorsManager>();

        IFuzzyInferenceSystem fuzzyInferenceSystem = new MamdaniFuzzyInference(new Minimum(), new Maximum(), new Minimum(),
                new Maximum(), new CentroidDefuzzifier());

        fuzzyEngine = new Engine(fuzzyInferenceSystem);

        Term close = new ZShapeTerm("close", 3.0, 8.0);
        Term far = new SShapeTerm("far", 7.0, 12.0);

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


        Term slow = new ZShapeTerm("slow", 0.0, 4.5);
        Term medium = new TrapezoidalTerm("medium", 4.0, 5.0, 7.0, 8.0);
        Term fast = new SShapeTerm("fast", 7.5, 9.0);

        LinguisticVariable speed = new LinguisticVariable("speed", 0.0, 10.0);
        speed.AddTerm(slow);
        speed.AddTerm(medium);
        speed.AddTerm(fast);
        fuzzyEngine.RegisterOutputVariable(speed);

        Term left = new ZShapeTerm("left", -35.0, -10.0);
        Term forward = new TrapezoidalTerm("forward", -15.0, -10.0, 10.0, 15.0);
        Term right = new SShapeTerm("right", 10.0, 35.0);

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
    }

    private void Update() {
        if (this.hasTarget) {
            fuzzyEngineInput["lls"] = this.sensorManager.SensorsOutput[0];
            fuzzyEngineInput["ls"] = this.sensorManager.SensorsOutput[1];
            fuzzyEngineInput["fs"] = this.sensorManager.SensorsOutput[2];
            fuzzyEngineInput["rs"] = this.sensorManager.SensorsOutput[3];
            fuzzyEngineInput["rrs"] = this.sensorManager.SensorsOutput[4];
            fuzzyEngineOutput = fuzzyEngine.Process(fuzzyEngineInput);
            Vector3 desiredDirection = this.GetMovementDirection(this.targetPosition);
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            float movementSpeed = this.GetMovementSpeed(this.targetPosition);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, this.turnSpeed * Time.deltaTime);
            if (Mathf.Approximately(this.characterController.velocity.magnitude, 0.0f)) {
                if (Quaternion.Angle(this.transform.rotation, targetRotation) < MIN_ANGLE_THRESHOLD) {
                    this.characterController.Move(this.transform.forward * movementSpeed * Time.deltaTime);
                }
            } else {
                this.characterController.Move(this.transform.forward * movementSpeed * Time.deltaTime);
            }

        }
    }

    private Vector3 GetMovementDirection(Vector3 targetPosition) {
        Vector3 desiredDirection = targetPosition - this.transform.position;
        float rotationAngle = (float)fuzzyEngineOutput["direction"];
        desiredDirection.y = 0.0f;
        desiredDirection = Quaternion.AngleAxis(rotationAngle, Vector3.up) * desiredDirection;
        return desiredDirection;
    }

    private float GetMovementSpeed(Vector3 targetPosition) {
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

    private void junk() {
        float rotationAngle = 0;
        if (this.sensorManager.SensorsOutput[0] < 7) {
            if (this.sensorManager.SensorsOutput[0] < 5) {
                rotationAngle += 15.0f;
            } else {
                rotationAngle += 5.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[1] < 7) {
            if (this.sensorManager.SensorsOutput[1] < 5) {
                rotationAngle += 25.0f;
            } else {
                rotationAngle += 15.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[2] < 7) {
            if (this.sensorManager.SensorsOutput[2] < 5) {
                rotationAngle += 45.0f;
            } else {
                rotationAngle += 15.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[3] < 7) {
            if (this.sensorManager.SensorsOutput[3] < 5) {
                rotationAngle -= 25.0f;
            } else {
                rotationAngle -= 15.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[4] < 7) {
            if (this.sensorManager.SensorsOutput[4] < 5) {
                rotationAngle -= 15.0f;
            } else {
                rotationAngle -= 5.0f;
            }
        }
        if (this.sensorManager.SensorsOutput[3] < 7) {
            rotationAngle -= 15.0f;
        }
        if (this.sensorManager.SensorsOutput[4] < 7) {
            rotationAngle -= 15.0f;
        }

    }
}
