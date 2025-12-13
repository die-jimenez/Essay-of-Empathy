using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BarreraData", menuName = "BarretaData", order = 1)]
public class BarreraData : ScriptableObject
{
    [Header("Estado A")]
    [Range(-5.5f, 4.5f)] public float alturaMin_A;
    [Range(-5.5f, 4.5f)] public float alturaMax_A;
    [Range(0, 20)] public float velocidadSubida_A;
    [Range(0, 20)] public float velocidadBajada_A;
    [Space(-5), Header("Avanzados")]
    [Range(0, 1)] public float rangoVariacionAltura_A;
    [Range(0, 2)] public float tiempoSostenidoEnMinimo_A;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo_A;

    [Space(40), Header("Estado B")]
    [Range(-5.5f, 4.5f)] public float alturaMin_B;
    [Range(-5.5f, 4.5f)] public float alturaMax_B;
    [Range(0, 20)] public float velocidadSubida_B;
    [Range(0, 20)] public float velocidadBajada_B;
    [Space(-5), Header("Avanzados")]
    [Range(0, 1f)] public float rangoVariacionAltura_B;
    [Range(0, 2)] public float tiempoSostenidoEnMinimo_B;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo_B;

    public void CopyDataFrom(BarreraData anotherData)
    {
        alturaMin_A = anotherData.alturaMin_A;
        alturaMax_A = anotherData.alturaMax_A;
        velocidadSubida_A = anotherData.velocidadSubida_A;
        velocidadBajada_A = anotherData.velocidadBajada_A;
        rangoVariacionAltura_A = anotherData.rangoVariacionAltura_A;
        tiempoSostenidoEnMinimo_A = anotherData.tiempoSostenidoEnMinimo_A;
        tiempoSostenidoEnMaximo_A = anotherData.tiempoSostenidoEnMaximo_A;

        alturaMin_B = anotherData.alturaMin_B;
        alturaMax_B = anotherData.alturaMax_B;
        velocidadSubida_B = anotherData.velocidadSubida_B;
        velocidadBajada_B = anotherData.velocidadBajada_B;
        rangoVariacionAltura_B = anotherData.rangoVariacionAltura_B;
        tiempoSostenidoEnMinimo_B = anotherData.tiempoSostenidoEnMinimo_B;
        tiempoSostenidoEnMaximo_B = anotherData.tiempoSostenidoEnMaximo_B;
    }
}
