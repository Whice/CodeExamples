using Data;
using Services;
using UI;
using UnityEngine;
using Utility;
using View;

public class World : MonoBehaviourLogger
{
    [SerializeField] private GameWindow gameWindow = null;
    [SerializeField] private GameField gameField = null;

    private readonly DataContainer dataContainer = new DataContainer();
    private readonly ServicesContainer servicesContainer = new ServicesContainer();

    public void Initialize()
    {
        servicesContainer.Initialize(dataContainer);

        gameWindow.Initialize(servicesContainer);
        gameField.Initialize(servicesContainer);
    }
    private void Awake()
    {
        SetLogPrefix(nameof(World));

        IsNullCheck(gameWindow, nameof(gameWindow));
        IsNullCheck(gameField, nameof(gameField));
        Initialize();
    }
}
