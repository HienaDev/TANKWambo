using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class MovementXY : Movement
{
    public enum InputType { Axis = 0, Button = 1, Key = 2};

    [SerializeField] 
    private Vector2     speed = new Vector2(100, 100);
    [SerializeField]
    private bool        useRotation = false;
    [SerializeField]
    private bool        inputEnabled;
    [SerializeField, ShowIf("inputEnabled")]
    private InputType   inputType;
    [SerializeField, ShowIf("axisEnabled"), InputAxis]
    private string      horizontalAxis = "Horizontal";
    [SerializeField, ShowIf("axisEnabled"), InputAxis]
    private string      verticalAxis = "Vertical";
    [SerializeField, ShowIf("buttonEnabled")]    
    private string      horizontalButtonPositive = "Right";
    [SerializeField, ShowIf("buttonEnabled")]    
    private string      horizontalButtonNegative = "Left";
    [SerializeField, ShowIf("buttonEnabled")]
    private string      verticalButtonPositive = "Up";
    [SerializeField, ShowIf("buttonEnabled")]
    private string      verticalButtonNegative = "Down";
    [SerializeField, ShowIf("keyEnabled")]    
    private KeyCode     horizontalKeyPositive = KeyCode.RightArrow;
    [SerializeField, ShowIf("keyEnabled")]
    private KeyCode     horizontalKeyNegative = KeyCode.LeftArrow;
    [SerializeField, ShowIf("keyEnabled")]    
    private KeyCode     verticalKeyPositive = KeyCode.UpArrow;
    [SerializeField, ShowIf("keyEnabled")]
    private KeyCode     verticalKeyNegative = KeyCode.DownArrow;

    private bool axisEnabled => inputEnabled && (inputType == InputType.Axis);
    private bool buttonEnabled => inputEnabled && (inputType == InputType.Button);
    private bool keyEnabled => inputEnabled && (inputType == InputType.Key);

    Vector3 moveVector;

    public override Vector2 GetSpeed() => speed;
    public override void    SetSpeed(Vector2 speed) { this.speed = speed; }

    override public string GetTitle() => "XY Movement";

    public override string GetRawDescription()
    {
        string desc = "";
        if (speed.x != 0.0f)
        {
            if (speed.y != 0.0f)
            {
                desc += $"Dual axis movement, at {speed} units per second.\n";
            }
            else
            {
                desc += $"Horizontal movement, at {speed.x} units per second.\n";
            }
        }
        else
        {
            if (speed.y != 0.0f)
            {
                desc += $"Vertical movement, at {speed.y} units per second.\n";
            }
            else
            {
                desc += $"No movement!\n";
            }
        }
        if (useRotation) desc += "These directions will be relative to the current object orientation.\n";
        if (inputEnabled)
        {
            if (inputType == InputType.Axis)
            {
                if ((horizontalAxis != "") && (horizontalAxis != "None"))
                {
                    desc += $"Horizontal movement will be controlled by the [{horizontalAxis}] axis.\n";
                }
                if ((verticalAxis!= "") && (verticalAxis != "None"))
                {
                    desc += $"Vertical movement will be controlled by the [{verticalAxis}] axis.\n";
                }
            }
            else if (inputType == InputType.Button)
            {
                if ((horizontalButtonPositive != "") || (horizontalButtonNegative != ""))
                {
                    desc += $"Horizontal movement will be controlled by the [{horizontalButtonNegative}] and [{horizontalButtonPositive}] buttons.\n";
                }
                if ((verticalButtonPositive != "") || (verticalButtonNegative != ""))
                {
                    desc += $"Vertical movement will be controlled by the [{verticalButtonNegative}] and [{verticalButtonPositive}] buttons.\n";
                }
            }
            else if (inputType == InputType.Key)
            {
                if ((horizontalKeyPositive != KeyCode.None) || (horizontalKeyNegative != KeyCode.None))
                {
                    desc += $"Horizontal movement will be controlled by the [{horizontalKeyNegative}] and [{horizontalKeyPositive}] keys.\n";
                }
                if ((verticalKeyPositive != KeyCode.None) || (verticalKeyNegative != KeyCode.None))
                {
                    desc += $"Vertical movement will be controlled by the [{verticalKeyNegative}] and [{verticalKeyPositive}] keys.\n";
                }
            }
        }
        return desc;
    }

    void FixedUpdate()
    {
        Vector3 transformedDelta = moveVector;
        if (useRotation)
        {
            transformedDelta = moveVector.x * transform.right + moveVector.y * transform.up;
        }

        MoveDelta(transformedDelta * Time.fixedDeltaTime);
    }

    void Update()
    {
        moveVector = Vector3.zero;
        if (inputEnabled)
        {
            switch (inputType)
            {
                case InputType.Axis:
                    if (horizontalAxis != "") moveVector.x = Input.GetAxis(horizontalAxis) * speed.x;
                    if (verticalAxis != "") moveVector.y = Input.GetAxis(verticalAxis) * speed.y;
                    break;
                case InputType.Button:
                    if ((horizontalButtonPositive != "") && (Input.GetButton(horizontalButtonPositive))) moveVector.x = speed.x;
                    if ((horizontalButtonNegative != "") && (Input.GetButton(horizontalButtonNegative))) moveVector.x = -speed.x;
                    if ((verticalButtonPositive != "") && (Input.GetButton(verticalButtonPositive))) moveVector.y = speed.y;
                    if ((verticalButtonNegative != "") && (Input.GetButton(verticalButtonNegative))) moveVector.y = -speed.y;
                    break;
                case InputType.Key:
                    if ((horizontalKeyPositive != KeyCode.None) && (Input.GetKey(horizontalKeyPositive))) moveVector.x = speed.x;
                    if ((horizontalKeyNegative != KeyCode.None) && (Input.GetKey(horizontalKeyNegative))) moveVector.x = -speed.x;
                    if ((verticalKeyPositive != KeyCode.None) && (Input.GetKey(verticalKeyPositive))) moveVector.y = speed.y;
                    if ((verticalKeyNegative != KeyCode.None) && (Input.GetKey(verticalKeyNegative))) moveVector.y = -speed.y;
                    break;
                default:
                    break;
            }
        }
        else
        {
            moveVector = speed;
        }
    }
}
