using System;
using UnityEngine;

public class DynamicVoltageAgent : InfinityCourseAgent
{

    public float R = 0.825f;
    public float l = 1.65f;

    private float speedL = 10f;
    private float speedR = 10f;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalMaterial = GetComponent<Renderer>().sharedMaterial;
        sensorManager = GetComponent<SensorsManager>();

        random = new System.Random();

        startSeconds = new TimeSpan(DateTime.Now.Ticks).TotalSeconds;
        transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        /*IFuzzyInferenceSystem fuzzyInferenceSystem = new MamdaniFuzzyInference(new Minimum(), new Maximum(), new Minimum(),
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
        fuzzyEngineInput.Add("rrs", 0.0);*/
    }

    private void Update()
    {
        if (this.hasTarget)
        {
            /*fuzzyEngineInput["lls"] = sensorManager.SensorsOutput[0];
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

            //characterController.Move(this.transform.forward * movementSpeed * Time.deltaTime * 0.55f);*/

            Vector2 target = new Vector2(100, 0);
            Vector2 right = new Vector2(1, 0);
            float currentRotation = transform.eulerAngles.y;
            Debug.Log("currentRotation = " + currentRotation);
            float angle = Vector2.SignedAngle(right, target / target.magnitude);
            float alpha = (90 - currentRotation - angle) * Time.deltaTime * turnSpeed;
            Debug.Log("Alpha = " + alpha);
            //скорости вращения для поворота
            float omegaL = 2 * (float)Math.Tan(alpha * Math.PI / 180) / R;
            float omegaR = -omegaL;

            double currentSeconds = new TimeSpan(DateTime.Now.Ticks).TotalSeconds - startSeconds;
            Vector2 speed = getSpeed(currentSeconds);

            float deltaV1 = speed.x * Time.deltaTime;
            float deltaV2 = speed.y * Time.deltaTime;
            Debug.Log(speed);

            omegaL = omegaL + deltaV1;
            omegaR = omegaR + deltaV2;

            Debug.Log("Left speed = " + omegaL + "; Right speed = " + omegaR);

            float v1 = R * omegaL;
            float v2 = R * omegaR;
            float phi = - currentRotation;
            float xNew = transform.position.x - 0.5f * (v2 + v1) * (float)Math.Sin(phi * Math.PI / 180);
            float yNew = transform.position.z + 0.5f * (v2 + v1) * (float)Math.Cos(phi * Math.PI / 180);
            float phiDif = (float)(Math.Atan((v2 - v1) / (2 * l)) * 180 / Math.PI);
            Debug.Log("PhiDiff " + phiDif);
            float phiNew = phi + phiDif;
            float newRotation = - phiNew;

            transform.rotation = Quaternion.AngleAxis(newRotation, Vector3.up);
            transform.position = new Vector3(xNew, transform.position.y, yNew);
        }
    }

    private Vector2 getSpeed(double seconds)
    {
        /*int iter = Convert.ToInt32(Math.Floor(seconds));
        Debug.Log("Iter = " + iter);
        if (iter < 3)
        {
            return new Vector2(10, 10);
        }
        if (iter < 5)
        {
            return new Vector2(10, 8);
        }
        if (iter < 7)
        {
            return new Vector2(10, 6);
        }
        if (iter < 9)
        {
            return new Vector2(10, 8);
        }
        if (iter < 11)
        {
            return new Vector2(10, 10);
        }
        if (iter < 13)
        {
            return new Vector2(8, 10);
        }
        if (iter < 15)
        {
            return new Vector2(6, 10);
        }
        if (iter < 17)
        {
            return new Vector2(8, 10);
        }*/
        return new Vector2(-10, -10);

    }

    private double startSeconds = 0;
    private System.Random random;
}