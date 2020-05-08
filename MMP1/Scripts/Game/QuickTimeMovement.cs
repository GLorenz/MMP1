
using Microsoft.Xna.Framework;
using System;

public class QuickTimeMovement
{
    public bool isActive { get; private set; }
    
    private static readonly float selectionAlpha = 0.5f;
    private static readonly long coolDownAfterInitiateMS = 250L;

    private DateTime initiated;

    private Vector2[] directions;
    private Meeple meeple;
    private PyramidFloorBoardElement start;
    private int curSelectionIdx;
    private Vector2 startCenter;

    private Vector2 curMouseVector;

    private QuickTimeMovement() { }

    public void Initiate(Meeple meeple)
    {
        this.meeple = meeple;
        this.start = meeple.standingOn;
        startCenter = start.Position.Center.ToVector2();
        curSelectionIdx = 0;
        start.connectedFields[curSelectionIdx].Highlight();

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

    public void ReceiveMousePos(Point mousePos)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            curMouseVector = mousePos.ToVector2() - startCenter;
            if (curSelectionIdx != i && Math.Acos(Vector2.Dot(curMouseVector, directions[i]) / curMouseVector.Length()) < selectionAlpha)
            {
                start.connectedFields[curSelectionIdx].Lowlight();
                start.connectedFields[i].Highlight();

                curSelectionIdx = i;
            }
        }
    }

    public void OnClick()
    {
        if ((DateTime.Now - initiated).TotalMilliseconds > coolDownAfterInitiateMS)
        {
            meeple.MoveTo(start.connectedFields[curSelectionIdx]);
            Quit();
        }
    }

    public void Quit()
    {
        isActive = false;
        start.connectedFields[curSelectionIdx].Lowlight();
    }

    public void Toggle(Meeple meeple)
    {
        if (isActive) Quit();
        else Initiate(meeple);
    }

    private static QuickTimeMovement qtm;
    public static QuickTimeMovement Instance()
    {
        if (qtm == null) qtm = new QuickTimeMovement();
        return qtm;
    }
}