using UnityEngine;

public class LightningService : IService
{
    GameObject electricitySphere;
    GameObject doubleElectricity;
    GuysBehaviorService guysBehaviorService;

    float electricityDistance = 10f;

    public void StartService(EngineScript instance)
    {
        doubleElectricity = UnityEngine.Object.Instantiate(Resources.Load("Prefabs\\DoubleElectricity") as GameObject);
        electricitySphere = UnityEngine.Object.Instantiate(Resources.Load("Prefabs\\ElectricitySphere") as GameObject);

        guysBehaviorService = instance.GetService<GuysBehaviorService>();
    }

    public void ShowDoubleLihgtning(Vector3 leftFingerPos, Vector3 rightFingerPos)
    {
        doubleElectricity.SetActive(true);
        doubleElectricity.transform.position   = new Vector3(leftFingerPos.x, leftFingerPos.y, doubleElectricity.transform.position.z);
        doubleElectricity.transform.localScale = new Vector3(Vector3.Distance(leftFingerPos, rightFingerPos) / electricityDistance, 1f, 1f);

        var turnAngle = Angle(leftFingerPos, rightFingerPos);

        var rotation = Quaternion.Euler(0, 0, turnAngle);
        doubleElectricity.transform.rotation = rotation;

        var origin    = leftFingerPos;
        var direction = (rightFingerPos - leftFingerPos).normalized * Vector3.Distance(leftFingerPos, rightFingerPos);
        origin.z      = 2f;
        direction.z   = 2f;

        RaycastHit[] hitInfos = Physics.RaycastAll(origin, direction, Vector3.Distance(leftFingerPos, rightFingerPos));
        foreach (var hitInfo in hitInfos)
        {
            guysBehaviorService.TouchGuyDoubleLightning(hitInfo.collider.transform.gameObject);
        }

        //var countParticle = 1000;
        //changeCountParticle(countParticle);
    }

    private float Angle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 from = pos2 - pos1;
        Vector2 to = new Vector2(1, 0);

        float result = Vector2.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        if (cross.z > 0)
        {
            result = 360f - result;
        }

        return result;
    }

    public void HideDoubleLihgtning()
    {
        doubleElectricity.SetActive(false);
    }

    public void ShowSphereLihgtning(Vector3 fingerPos)
    {
        electricitySphere.SetActive(true);
        electricitySphere.transform.position = new Vector3(fingerPos.x, fingerPos.y, doubleElectricity.transform.position.z);

        var origin = fingerPos;
        origin.z   = 0f;

        var direction = Vector3.forward * 2;

        RaycastHit hitInfo;
        if (Physics.Raycast(fingerPos, direction, out hitInfo))
        {
            guysBehaviorService.TouchGuy(hitInfo.collider.transform.gameObject);
        }
    }

    public void HideSphereLihgtning()
    {
        electricitySphere.SetActive(false);
    }

    //private void changeCountParticle(int count)
    //{
    //    var mainLeft = electricityLeft.GetComponent<ParticleSystem>().main;
    //    mainLeft.maxParticles = count;

    //    var mainRight = electricityRight.GetComponent<ParticleSystem>().main;
    //    mainRight.maxParticles = count;
    //}
}
