// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;

public abstract class MovingBoardElement : TexturedBoardElement, IVisibleBoardElement
{
    protected Vector2 currentLocation;
    protected Vector2 moveTowards;
    protected bool isMoving;
    protected float moveSpeed = 0.2f;
    private static readonly int frameRateMS = 1000 / 60;

    public MovingBoardElement(Rectangle position, Texture2D texture, string UID, int zPosition = 0) : base(position, texture, UID, zPosition) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        DrawDefault(spriteBatch);
    }

    public virtual void MoveToAndShare(PyramidFloorBoardElement element)
    {
        MoveMBECommand cmd = new MoveMBECommand(this, element);
        PlayerManager.Instance().local.HandleInput(cmd, true);
    }

    protected async void MoveTowardsTarget()
    {
        isMoving = true;
        currentLocation = position.Location.ToVector2();

        while ((currentLocation - moveTowards).Length() > 3)
        {
            currentLocation = Vector2.Lerp(currentLocation, moveTowards, moveSpeed);
            this.position.Location = currentLocation.ToPoint();
            await Task.Delay(frameRateMS);
        }

        this.position.Location = moveTowards.ToPoint();
        isMoving = false;
    }

    public void MoveToLocalOnly(Point position)
    {
        moveTowards = position.ToVector2();
        if (!isMoving)
        {
            new Task(() => MoveTowardsTarget()).Start();
        }
    }

    public virtual void MoveToLocalOnly(PyramidFloorBoardElement element)
    {
        MoveToLocalOnly(element.Position.Location);
    }

    public void MoveToLocalOnlyDirect(Point position)
    {
        this.position.Location = position;
    }
}