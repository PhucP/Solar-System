using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Movement;
using Data;
using UnityEngine;

public class ForceManager
{
    private List<CelestialObject> _listCelestials;
    private CelestialObjectData _planetData;

    public ForceManager(List<CelestialObject> listCelestials, CelestialObjectData data)
    {
        this._listCelestials = listCelestials;
        this._planetData = data;
    }
    
    public Vector3 CalculateGravityForce(Vector3 planetPosition)
    {
        Vector3 tempVel = Vector3.zero;
        foreach (CelestialObject otherPlanet in _listCelestials)
        {
            if (_planetData.infomation.name.Equals(otherPlanet.celestialObjectData.infomation.name)) continue;

            var vectorDistance = otherPlanet.transform.position - planetPosition;
            float sqrDst = vectorDistance.sqrMagnitude;
            Vector3 forceDir = vectorDistance.normalized;

            float planetMass = _planetData.physic.mass;
            float otherPlanetMass = otherPlanet.celestialObjectData.physic.mass;
            Vector3 force = forceDir * (Constant.G * planetMass * otherPlanetMass) / sqrDst;
            Vector3 acceleration = force / planetMass;

            tempVel += acceleration * CelestialManager.Instance.timeStep;
        }

        return tempVel;
    }
    
    public Vector3 CalculateInitialForce(CelestialObject planet)
    {
        Vector3 resForce = Vector3.zero;
        foreach (var otherPlanet in _listCelestials)
        {
            if (planet.Equals(otherPlanet)) continue;
            var otherPlanetMass = planet.celestialObjectData.physic.mass;
            float r = Vector3.Distance(planet.transform.position, otherPlanet.transform.position);

            Transform transform;
            (transform = planet.transform).LookAt(planet.transform);

            resForce += transform.right * Mathf.Sqrt((Constant.G * otherPlanetMass) / r);
        }

        return resForce;
    }
}
