using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetProblemInfo : MonoBehaviour
{
    public BoxCollider ColisorPlataformaInicial;
    public BoxCollider ColisorPlataformaDoMeio;
    public BoxCollider ColisorPlataformaFinal;
    public GameObject CentroDoRaycastDoPulo;
    public SetProblemInfo setProblemInfo;

    [SerializeField]
    private Vector3 dimensaoPlataformaInicial = Vector3.zero;
    [SerializeField]
    private Vector3 dimensaoPlataformaMeio = Vector3.zero;
    [SerializeField]
    private Vector3 dimensaoPlataformaFinal = Vector3.zero;
    [SerializeField]
    private Vector3 distanciaEntreInicialEPulo = Vector3.zero;
    [SerializeField]
    private Vector3 distanciaEntrePuloEMeio = Vector3.zero;
    [SerializeField]
    private Vector3 distanciaEntreMeioEFinal = Vector3.zero;

    private void Start() {
        dimensaoPlataformaInicial = ColisorPlataformaInicial.bounds.size;
        dimensaoPlataformaMeio = ColisorPlataformaDoMeio.bounds.size;
        dimensaoPlataformaFinal = ColisorPlataformaFinal.bounds.size;
        distanciaEntreInicialEPulo = CentroDoRaycastDoPulo.transform.position - ColisorPlataformaInicial.bounds.center;
        distanciaEntrePuloEMeio = ColisorPlataformaDoMeio.bounds.center - CentroDoRaycastDoPulo.transform.position;
        distanciaEntreMeioEFinal = ColisorPlataformaFinal.bounds.center - ColisorPlataformaDoMeio.bounds.center;
    }

    public void OnIntialPlatformChange(){
        distanciaEntreInicialEPulo = CentroDoRaycastDoPulo.transform.position - ColisorPlataformaInicial.bounds.center;
        setProblemInfo.OnInfoChanged(this);
    }


    public Vector3 GetDimensaoPlataformaInicial(){
        return dimensaoPlataformaInicial;
    }

    public Vector3 GetDimensaoPlataformaMeio(){
        return dimensaoPlataformaMeio;
    }

    public Vector3 GetDimensaoPlataformaFinal(){
        return dimensaoPlataformaFinal;
    }

    public Vector3 GetDistanciaEntreInicialEPulo(){
        return distanciaEntreInicialEPulo;
    }

    public Vector3 GetDistanciaEntrePuloEMeio(){
        return distanciaEntrePuloEMeio;
    }

    public Vector3 GetDistanciaEntreMeioEFinal(){
        return distanciaEntreMeioEFinal;
    }

}
