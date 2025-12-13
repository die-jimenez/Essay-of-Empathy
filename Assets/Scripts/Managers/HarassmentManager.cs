using UnityEngine;

public class HarassmentManager : MonoBehaviour
{
    public static HarassmentManager instance;
    private const float MinBarHeight = -3f;
    private Barra _protagonist;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.puntero.Pressed.AddListener(OnPointerPressed);
        GameManager.instance.puntero.Released.AddListener(OnPointerReleased);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(ChangeBackground);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(OnBarHovered);

        _protagonist = GameManager.instance.protagonista;
    }

    private void Update()
    {
        if (GameManager.instance.hayInterccion) return;
        foreach (Barra barra in GameManager.instance.barras)
        {
            ResetBarToInitialPosition(barra);

            bool isAtStartingPos = barra.direccion ==
                                   Barra.Direccion.sube &&
                                   barra.estado == Barra.Estado.transicion;
            if (isAtStartingPos) barra.GetValoresEstadoA();
        }
    }
    
    
    private void OnPointerPressed()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            barra.GetValoresEstadoB();
            barra.movimiento = Barra.Movimiento.manual;
        }
    }
    
    private void OnPointerReleased()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }

    private void ResetBarToInitialPosition(Barra bar)
    {
        if (bar.movimiento == Barra.Movimiento.manual)
        {
            bar.movimiento = Barra.Movimiento.automatico;
            bar.direccion = Barra.Direccion.baja;
            bar.GetValoresEstadoA();

            //Debe ir después de GetValoresA
            if (bar != GameManager.instance.protagonista) bar.velocidadBajada = 10f;
            else bar.velocidadBajada = 4f;

            bar.estado = Barra.Estado.transicion;
        }
    }

    private void ChangeBackground(Barra hoveredBar)
    {
        if (hoveredBar.index == _protagonist.index)
        {
            GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
        }
        else GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }

    void OnBarHovered(Barra hoveredBar)
    {
        int protagonistIndex = _protagonist.index;
        int totalBars = GameManager.instance.barras.Count;
        float verticalOffset = VerticalOffset(hoveredBar);

        _protagonist.heightTarget = _protagonist.data.alturaMax_A - 1;
        SetLeftBarsHeightOnFinalPos(protagonistIndex, verticalOffset, MinBarHeight);
        SetRightBarsHeightOnFinalPos(protagonistIndex, totalBars, verticalOffset, MinBarHeight);
    }

    private float VerticalOffset(Barra bar)
    {
        float baseYOffset = 0.1f;
        int protagonistIndex = _protagonist.index;
        bool isOnLeftOfProtagonist = bar.index <= protagonistIndex - 1 && bar.index != protagonistIndex;

        if (bar.index == protagonistIndex)
            return 0.65f;

        if (isOnLeftOfProtagonist) return baseYOffset + (bar.index / 30f);
        else return baseYOffset + ((GameManager.instance.barras.Count - bar.index) / 30f);
    }

    private void SetLeftBarsHeightOnFinalPos(int protagonistIndex, float verticalOffset, float minHeight)
    {
        var bars = GameManager.instance.barras;

        for (int i = protagonistIndex - 1; i >= 0; i--)
        {
            int distanceFromProtagonist = protagonistIndex - i;
            bars[i].heightTarget = minHeight + (distanceFromProtagonist * verticalOffset);
        }
    }

    private void SetRightBarsHeightOnFinalPos(int protagonistIndex, int totalBars, float spacing, float minHeight)
    {
        var bars = GameManager.instance.barras;

        for (int i = protagonistIndex + 1; i < totalBars; i++)
        {
            int distanceFromProtagonist = Mathf.Abs(protagonistIndex - i);
            bars[i].heightTarget = minHeight + (distanceFromProtagonist * spacing);
        }
    }


}