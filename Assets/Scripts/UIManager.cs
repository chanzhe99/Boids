using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] BoidManager BM;

    // Declare Settings variables
    bool Settings = false;

    // Declare Spawn Boids variables
    [Header("Spawn Boids Settings")] [Space(5)]
    [SerializeField] GameObject SpawnBoidsPanel;
    [SerializeField] int SpawnBoidsMin = 1;
    [SerializeField] int SpawnBoidsMax = 200;
    Slider SpawnBoidsSlider;
    TMP_Text SpawnBoidsText;

    // Declare Scan Radius variables
    [Header("Slider Panels")] [Space(5)]
    [SerializeField] GameObject ScanRadiusPanel;
    float ScanRadiusMin = 0;
    float ScanRadiusMax = 1;
    Slider ScanRadiusSlider;
    TMP_Text ScanRadiusText;

    // Declare Separation Radius variables
    [SerializeField] GameObject SeparationRadiusPanel;
    float SeparationRadiusMin = 0;
    float SeparationRadiusMax = 1;
    Slider SeparationRadiusSlider;
    TMP_Text SeparationRadiusText;

    // Declare Scan Angle variables
    [SerializeField] GameObject ScanAnglePanel;
    float ScanAngleMin = 0;
    float ScanAngleMax = 180;
    Slider ScanAngleSlider;
    TMP_Text ScanAngleText;

    // Declare Separation Weight variables
    [SerializeField] GameObject SeparationWeightPanel;
    float SeparationWeightMin = 0;
    float SeparationWeightMax = 5;
    Slider SeparationWeightSlider;
    TMP_Text SeparationWeightText;

    // Declare Alignment Radius variables
    [SerializeField] GameObject AlignmentWeightPanel;
    float AlignmentWeightMin = 0;
    float AlignmentWeightMax = 5;
    Slider AlignmentWeightSlider;
    TMP_Text AlignmentWeightText;

    // Declare Cohesion Radius variables
    [SerializeField] GameObject CohesionWeightPanel;
    float CohesionWeightMin = 0;
    float CohesionWeightMax = 5;
    Slider CohesionWeightSlider;
    TMP_Text CohesionWeightText;

    // Declare Toggles
    [Header("Toggle Panels")] [Space(5)]
    [SerializeField] GameObject AttractorPanel;
    [SerializeField] GameObject ObstaclePanel;
    [SerializeField] GameObject RepellantPanel;
    [SerializeField] GameObject ScreenWrapPanel;
    Toggle AttractorToggle;
    Toggle ObstacleToggle;
    Toggle RepellantToggle;
    Toggle ScreenWrapToggle;

    //Declare Instruction Text
    [Header("Instructions Text")] [Space(5)]
    [SerializeField] GameObject InstructionText;

    void Start()
    {
        // Get Spawn Boids UI components
        SpawnBoidsSlider = SpawnBoidsPanel.GetComponentInChildren<Slider>();
        SpawnBoidsText = SpawnBoidsPanel.GetComponentInChildren<TMP_Text>();

        // Get Scan Radius UI components
        ScanRadiusSlider = ScanRadiusPanel.GetComponentInChildren<Slider>();
        ScanRadiusText = ScanRadiusPanel.GetComponentInChildren<TMP_Text>();

        // Get Separation Radius UI components
        SeparationRadiusSlider = SeparationRadiusPanel.GetComponentInChildren<Slider>();
        SeparationRadiusText = SeparationRadiusPanel.GetComponentInChildren<TMP_Text>();

        // Get Scan Angle UI components
        ScanAngleSlider = ScanAnglePanel.GetComponentInChildren<Slider>();
        ScanAngleText = ScanAnglePanel.GetComponentInChildren<TMP_Text>();

        // Get Separation Weight UI components
        SeparationWeightSlider = SeparationWeightPanel.GetComponentInChildren<Slider>();
        SeparationWeightText = SeparationWeightPanel.GetComponentInChildren<TMP_Text>();

        // Get Alignment Weight UI components
        AlignmentWeightSlider = AlignmentWeightPanel.GetComponentInChildren<Slider>();
        AlignmentWeightText = AlignmentWeightPanel.GetComponentInChildren<TMP_Text>();

        // Get Cohesion Weight UI components
        CohesionWeightSlider = CohesionWeightPanel.GetComponentInChildren<Slider>();
        CohesionWeightText = CohesionWeightPanel.GetComponentInChildren<TMP_Text>();

        // Get Toggle components
        AttractorToggle = AttractorPanel.GetComponentInChildren<Toggle>();
        ObstacleToggle = ObstaclePanel.GetComponentInChildren<Toggle>();
        RepellantToggle = RepellantPanel.GetComponentInChildren<Toggle>();
        ScreenWrapToggle = ScreenWrapPanel.GetComponentInChildren<Toggle>();

        // Initiate Settings UI values
        Settings = false;
        SpawnBoidsPanel.SetActive(false);
        ScanRadiusPanel.SetActive(false);
        SeparationRadiusPanel.SetActive(false);
        ScanAnglePanel.SetActive(false);
        SeparationWeightPanel.SetActive(false);
        AlignmentWeightPanel.SetActive(false);
        CohesionWeightPanel.SetActive(false);
        AttractorPanel.SetActive(false);
        ObstaclePanel.SetActive(false);
        RepellantPanel.SetActive(false);
        ScreenWrapPanel.SetActive(false);
        InstructionText.SetActive(false);

        // Initiate Spawn Boids UI values
        SpawnBoidsSlider.minValue = SpawnBoidsMin;
        SpawnBoidsSlider.maxValue = SpawnBoidsMax;
        SpawnBoidsSlider.value = BM.UI_BoidsToSpawn;

        // Initiate Scan Radius UI values
        ScanRadiusSlider.minValue = ScanRadiusMin;
        ScanRadiusSlider.maxValue = ScanRadiusMax;
        ScanRadiusSlider.value = BM.UI_ScanRadius;

        // Initiate Separation Radius UI values
        SeparationRadiusSlider.minValue = SeparationRadiusMin;
        SeparationRadiusSlider.maxValue = SeparationRadiusMax;
        SeparationRadiusSlider.value = BM.UI_SeparationRadius;

        // Initiate Scan Angle UI values
        ScanAngleSlider.minValue = ScanAngleMin;
        ScanAngleSlider.maxValue = ScanAngleMax;
        ScanAngleSlider.value = BM.UI_ScanAngle;

        // Initiate Separation Weight UI values
        SeparationWeightSlider.minValue = SeparationWeightMin;
        SeparationWeightSlider.maxValue = SeparationWeightMax;
        SeparationWeightSlider.value = BM.UI_SeparationWeight;

        // Initiate Alignment Weight UI values
        AlignmentWeightSlider.minValue = AlignmentWeightMin;
        AlignmentWeightSlider.maxValue = AlignmentWeightMax;
        AlignmentWeightSlider.value = BM.UI_AlignmentWeight;

        // Initiate Cohesion Weight UI values
        CohesionWeightSlider.minValue = CohesionWeightMin;
        CohesionWeightSlider.maxValue = CohesionWeightMax;
        CohesionWeightSlider.value = BM.UI_CohesionWeight;

        // Initiate Toggle values
        AttractorToggle.isOn = BM.UI_IsSeeking;
        ObstacleToggle.isOn = BM.UI_IsAvoiding;
        RepellantToggle.isOn = BM.UI_IsBeingRepelled;
        ScreenWrapToggle.isOn = BM.UI_IsScreenWrapping;

        SpawnBoidsText.SetText(SpawnBoidsSlider.value.ToString("Boids: 000"));
        ScanRadiusText.SetText(ScanRadiusSlider.value.ToString("Scan Radius:\n0.00"));
        SeparationRadiusText.SetText(SeparationRadiusSlider.value.ToString("Separation\nRadius: 0%"));
        ScanAngleText.SetText((ScanAngleSlider.value * 2).ToString("Scan Angle:\n0"));
        SeparationWeightText.SetText(SeparationWeightSlider.value.ToString("Separation Weight: 0%"));
        AlignmentWeightText.SetText(AlignmentWeightSlider.value.ToString("Alignment Weight: 0%"));
        CohesionWeightText.SetText(CohesionWeightSlider.value.ToString("Cohesion Weight: 0%"));
    }
    
    public void ButtonSettings()
    {
        if(!Settings)
        {
            Settings = true;
            SpawnBoidsPanel.SetActive(true);
            ScanRadiusPanel.SetActive(true);
            SeparationRadiusPanel.SetActive(true);
            ScanAnglePanel.SetActive(true);
            SeparationWeightPanel.SetActive(true);
            AlignmentWeightPanel.SetActive(true);
            CohesionWeightPanel.SetActive(true);
            AttractorPanel.SetActive(true);
            ObstaclePanel.SetActive(true);
            RepellantPanel.SetActive(true);
            ScreenWrapPanel.SetActive(true);
            InstructionText.SetActive(true);
        }
        else
        {
            Settings = false;
            SpawnBoidsPanel.SetActive(false);
            ScanRadiusPanel.SetActive(false);
            SeparationRadiusPanel.SetActive(false);
            ScanAnglePanel.SetActive(false);
            SeparationWeightPanel.SetActive(false);
            AlignmentWeightPanel.SetActive(false);
            CohesionWeightPanel.SetActive(false);
            AttractorPanel.SetActive(false);
            ObstaclePanel.SetActive(false);
            RepellantPanel.SetActive(false);
            ScreenWrapPanel.SetActive(false);
            InstructionText.SetActive(false);
        }
    }

    public void SliderSpawnBoids()
    {
        BM.UI_BoidsToSpawn = (int)SpawnBoidsSlider.value;
        SpawnBoidsText.SetText(SpawnBoidsSlider.value.ToString("Boids: 000"));
    }

    public void SliderScanRadius()
    {
        BM.UI_ScanRadius = ScanRadiusSlider.value;
        ScanRadiusText.SetText(ScanRadiusSlider.value.ToString("Scan Radius:\n0.00"));
    }

    public void SliderSeparationRadius()
    {
        BM.UI_SeparationRadius = SeparationRadiusSlider.value;
        SeparationRadiusText.SetText(SeparationRadiusSlider.value.ToString("Separation\nRadius: 0%"));
    }

    public void SliderScanAngle()
    {
        BM.UI_ScanAngle = ScanAngleSlider.value;
        ScanAngleText.SetText((ScanAngleSlider.value * 2).ToString("Scan Angle:\n0"));
    }

    public void SliderSeparationWeight()
    {
        BM.UI_SeparationWeight = SeparationWeightSlider.value;
        SeparationWeightText.SetText(SeparationWeightSlider.value.ToString("Separation Weight: 0%"));
    }

    public void SliderAlignmentWeight()
    {
        BM.UI_AlignmentWeight = AlignmentWeightSlider.value;
        AlignmentWeightText.SetText(AlignmentWeightSlider.value.ToString("Alignment Weight: 0%"));
    }

    public void SliderCohesionWeight()
    {
        BM.UI_CohesionWeight = CohesionWeightSlider.value;
        CohesionWeightText.SetText(CohesionWeightSlider.value.ToString("Cohesion Weight: 0%"));
    }

    public void ToggleAttractor() { BM.UI_IsSeeking = AttractorToggle.isOn; }
    public void ToggleObstacle() { BM.UI_IsAvoiding = ObstacleToggle.isOn; }
    public void ToggleRepellant() { BM.UI_IsBeingRepelled = RepellantToggle.isOn; }
    public void ToggleScreenWrap() { BM.UI_IsScreenWrapping = ScreenWrapToggle.isOn; }
    public void ButtonSpawnBoids() { BM.SpawnBoids(); }
    public void ButtonQuit() { Application.Quit(); }
}
