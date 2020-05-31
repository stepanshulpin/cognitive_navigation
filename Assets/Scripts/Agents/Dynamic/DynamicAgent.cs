using AI.FuzzyLogic;
using AI.FuzzyLogic.FuzzyInference;
using AI.FuzzyLogic.Terms;
using System;
using System.Collections.Generic;
using Unity.UNetWeaver;
using UnityEngine;

public class DynamicAgent : InfinityCourseAgent
{   

    private void Awake()
    {
        previousPoint = transform.position;
        characterController = GetComponent<CharacterController>();
        originalMaterial = GetComponent<Renderer>().sharedMaterial;
        sensorManager = GetComponent<SensorsManager>();

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

    private void Update()
    {
        if (this.hasTarget)
        {
            fuzzyEngineInput["lls"] = sensorManager.SensorsOutput[0];
            fuzzyEngineInput["ls"] = sensorManager.SensorsOutput[1];
            fuzzyEngineInput["fs"] = sensorManager.SensorsOutput[2];
            fuzzyEngineInput["rs"] = sensorManager.SensorsOutput[3];
            fuzzyEngineInput["rrs"] = sensorManager.SensorsOutput[4];
            fuzzyEngineOutput = fuzzyEngine.Process(fuzzyEngineInput);
            Vector3 desiredDirection = GetMovementDirection(targetPosition);
            Debug.Log("desired dir = " + desiredDirection);
            float angleFromReg = GetAngle();
            Debug.Log("angle from reg = " + angleFromReg);
            desiredDirection.y = previousPoint.y;
            float movementSpeed = GetMovementSpeed(targetPosition);
            Vector2 target = new Vector2(desiredDirection.x, desiredDirection.z);
            Vector2 right = new Vector2(1, 0);
            float angle = Vector2.SignedAngle(right, target / target.magnitude);
            transform.rotation = Quaternion.AngleAxis(90 - angle, Vector3.up);
            Debug.Log("angle = " + angle + " speed = " + movementSpeed);
            double tgAlpha = Math.Tan(angle * Math.PI / 180);
            double tgAlphaSqr = Math.Pow(tgAlpha, 2);
            double speedSqr = Math.Pow(movementSpeed, 2);
            float x = (float)Math.Abs(Math.Sqrt(speedSqr / (tgAlphaSqr + 1)));
            float z = (float)(x * tgAlpha);
            Debug.Log(transform.forward);
            Debug.Log("x=" + x + " " + "z=" + z);
            transform.position = new Vector3(previousPoint.x + x * Time.deltaTime, previousPoint.y, previousPoint.z + z * Time.deltaTime);
            
            //characterController.Move(this.transform.forward * movementSpeed * Time.deltaTime * 0.55f);
            previousPoint = transform.position;

        }
    }

    private float GetAngle()
    {
        return (float)fuzzyEngineOutput["direction"];
    }

    private Vector3 previousPoint;
}