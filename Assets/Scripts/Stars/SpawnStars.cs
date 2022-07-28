using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpawnStars : MonoBehaviour
{
    [Tooltip("Minimum distance between points.")]
    public float poissonRadius = 1;
    public int rejectionSamples = 10;

    [Tooltip("Gizmo display radius.")]
    public float dispRadius = 0.1f;
    public int maxStarCount = 1500;

    ParticleSystem.Particle[] particles;
    ParticleSystem system;

    Vector3[] points;

    // method called when values are changed in inspector
    void OnValidate()
    {
        RegenerateStars();
    }

    public void RegenerateStars()
    {
        system = GetComponent<ParticleSystem>();
        points = StarGeneration.GeneratePoints(poissonRadius, system.shape.scale, rejectionSamples, 0, system.shape.scale.y/2, maxStarCount);
    }

    // drawing spheres at the points
    // and drawing the generation area
    void OnDrawGizmos()
    {
        if (points != null)
        {
            foreach (Vector3 point in points)
            { 
                //convert point to world coordinates
                Gizmos.DrawSphere(transform.TransformPoint(point), dispRadius);
                //Gizmos.DrawSphere(point, dispRadius);
            }
        }
    }

    void Start()
    {
        system = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[maxStarCount];
        system.Emit(points.Length);
        InvokeRepeating(nameof(SetParticlePosition), 0f, 1f);
    }

    void SetParticlePosition()
    {
        points = StarGeneration.GeneratePoints(poissonRadius, system.shape.scale, rejectionSamples, 0, system.shape.scale.y/2, maxStarCount);
        Debug.Log("Generated " + points.Length + " points");
        if (points != null)
        {
            system.GetParticles(particles);
            for (int i = 0; i < points.Length; ++i)
            {
                particles[i].position = points[i];            
            }
            system.SetParticles(particles, points.Length);
        }
    }
}
