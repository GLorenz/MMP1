﻿// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

public class QuickTimeMovement
{
    public bool isActive { get; private set; }
    
    private static readonly float selectionAlpha = 0.4f;
    private static readonly long coolDownAfterInitiateMS = 250L;
    private static readonly float arrowSpeed = 10f;

    private DateTime initiated;

    private Vector2[] directions;
    private Meeple meeple;
    private PyramidFloorBoardElement start;
    private Vector2 startCenter;

    private int curSelectionIdx;
    private Vector2 curArrowDirection;
    private ArrowAnimatable curArrow;

    private delegate void LoopedAction(int i);

    private QuickTimeMovement() { }

    private void Initiate(Meeple meeple)
    {
        this.meeple = meeple;
        this.start = meeple.standingOn;
        startCenter = start.Position.Center.ToVector2();
        curArrow = new ArrowAnimatable(start.Position, "movementarrow", start.ZPosition+1);
        CommandQueue.Queue(new AddToBoardCommand(curArrow));

        // build direction vectors to each connected field
        directions = new Vector2[start.connectedFields.Count];
        for(int i = 0; i < directions.Length; i++)
        {
            directions[i] = start.connectedFields[i].Position.Center.ToVector2() - startCenter;
            directions[i].Normalize();
        }

        isActive = true;
        initiated = DateTime.Now;
    }

    public void Update()
    {
        AnimateRotation();
    }

    public void AnimateRotation()
    {
        curArrowDirection.X = (float)Math.Cos(curArrow.GetAngle());
        curArrowDirection.Y = (float)Math.Sin(curArrow.GetAngle());

        DoActionIfHoverOrNot(curArrowDirection,
            // if hover
            (i) =>
            {
                start.connectedFields[i].Hover();
            },
            // if not hover
            (i) =>
            {
                start.connectedFields[i].DeHover();
            }
        );
        
        curArrow.AnimateAngle((float)(DateTime.Now - initiated).TotalSeconds * arrowSpeed);
    }

    public void OnClick()
    {
        if ((DateTime.Now - initiated).TotalMilliseconds > coolDownAfterInitiateMS)
        {
            if (DoActionIfHoverOrNot(curArrowDirection, null, null))
            {
                meeple.MoveToAndShare(start.connectedFields[curSelectionIdx]);
            }
            Quit();
        }
    }

    private bool DoActionIfHoverOrNot(Vector2 dir, LoopedAction hoverAction, LoopedAction nonHoverAction)
    {
        bool hoverAny = false;
        for (int i = 0; i < directions.Length; i++)
        {
            if (Math.Acos(Vector2.Dot(dir, directions[i]) / dir.Length()) < selectionAlpha)
            {
                hoverAny = true;
                hoverAction?.Invoke(i);
                curSelectionIdx = i;
            }
            else
            {
                nonHoverAction?.Invoke(i);
            }
        }
        return hoverAny;
    }

    public void Quit()
    {
        isActive = false;
        start.connectedFields[curSelectionIdx].DeHover();
        CommandQueue.Queue(new RemoveFromBoardCommand(curArrow));
    }

    private async void RemoveArrowDelayed(ArrowAnimatable arrow)
    {
        await Task.Delay(500);
        CommandQueue.Queue(new RemoveFromBoardCommand(arrow));
    }

    public void Activate(Meeple meeple)
    {
        if(!isActive)
        {
            Initiate(meeple);
        }
    }

    private static QuickTimeMovement qtm;
    public static QuickTimeMovement Instance()
    {
        if (qtm == null) qtm = new QuickTimeMovement();
        return qtm;
    }
}