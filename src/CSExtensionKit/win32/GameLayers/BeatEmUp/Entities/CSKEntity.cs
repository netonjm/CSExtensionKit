using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{

	public enum LifePosition
	{
		Right = 1,
		Left = 2,
		Down = 3,
		Up = 4
	}

	public class CSKEntity : CCSprite
	{

		public CCSprite Portrait { get; set; } //This portrait is showed on health

		public CCAction hurtAction { get; set; }
		public CCAction attackAction { get; set; }
		public CCAction idleAction { get; set; }
		public CCAction knockedOutAction { get; set; }
		public CCAction movementAction { get; set; }

		public bool IsOnRight(CSKEnemy enemy)
		{
			return PositionX > enemy.PositionX;
		}
		public bool IsOnLeft(CSKEnemy enemy)
		{
			return PositionX < enemy.PositionX;
		}

		public bool IsOnTop(CSKEnemy enemy)
		{
			return PositionY < enemy.PositionX;
		}

		public bool IsOnBottom(CSKEnemy enemy)
		{
			return PositionY > enemy.PositionX;
		}

		public void SetFlip(bool value)
		{
			FlipX = value;

			if (value)
				attackBox.Position = new CCPoint(centerToSides, -10);
			else
				attackBox.Position = new CCPoint(-(centerToSides + attackBox.actual.Size.Width), -10);

		}

		//collision boxes
		public BoundingBox hitBox;

		public BoundingBox attackBox;

		//states
		public ActionState actionState;

		//attributes
		public float movementSpeed;
		public float hitPoints;
		public float damage;

		//movement
		public CCPoint velocity;
		public CCPoint desiredPosition;

		//measurements
		public float centerToSides;
		public float centerToBottom;

		public CCPoint AbsoluteHitBoxPosition
		{
			get
			{
				return (Position + hitBox.actual.Origin);
			}
		}

		public CCRect AbsoluteHitBoxRect
		{

			get
			{
				return CreateBoundingBox(AbsoluteHitBoxPosition, hitBox.actual.Size);
			}
		}

		public CCPoint AbsoluteAttackBoxPosition
		{
			get
			{
				return (Position + attackBox.actual.Origin);
			}
		}

		public CCRect AbsoluteAttackBoxRect
		{

			get
			{
				return CreateBoundingBox(AbsoluteAttackBoxPosition, attackBox.actual.Size);
			}
		}


		public CCRect CreateBoundingBox(CCPoint position, CCSize size)
		{
			return new CCRect(position.X, position.Y, size.Width, size.Height);
		}

		public CCRect CreateBoundingBox(CCPoint position, CCRect rect)
		{
			return CreateBoundingBox(position, rect.Size);
			//return new CCRect(position.X, position.Y, size.Width, size.Height);
		}

		public CSKEntity(string image)
			: base(image)
		{

		}

		public CCPoint GetLifePosition(LifePosition position, int offset = 0)
		{

			switch (position)
			{
				case LifePosition.Right:
					return Position + new CCPoint(hitBox.actual.Size.Width + offset, 0);
				case LifePosition.Left:
					return Position - new CCPoint(hitBox.actual.Size.Width + offset, 0);
				case LifePosition.Down:
					return Position - new CCPoint(0, hitBox.actual.Size.Height + offset);
				default:
					return Position + new CCPoint(0, hitBox.actual.Size.Height + offset);
			}
		}

		//action methods
		public void idle()
		{
			if (actionState != ActionState.Idle)
			{
				StopAllActions();
				RunAction(idleAction);
				actionState = ActionState.Idle;
				velocity = CCPoint.Zero;
			}
		}

		public void attack()
		{
			if (actionState == ActionState.Idle || actionState == ActionState.Attack || actionState == ActionState.Walk)
			{
				StopAllActions();

				if (attackAction != null)
					RunAction(attackAction);

				actionState = ActionState.Attack;
			}
		}

		public void hurtWithDamage(float damage)
		{
			if (actionState != ActionState.KnockedOut)
			{

				StopAllActions();

				if (hurtAction != null)
					RunAction(hurtAction);

				actionState = ActionState.Hurt;

				hitPoints -= damage;

				if (hitPoints <= 0.0)
					knockout();
			}
		}


		public virtual void knockout()
		{
			StopAllActions();

			if (knockedOutAction != null)
				RunAction(knockedOutAction);

			hitPoints = 0.0f;
			actionState = ActionState.KnockedOut;
		}

		public void moveWithDirection(CCPoint direction)
		{
			if (actionState == ActionState.Idle)
			{
				StopAllActions();

				if (movementAction != null)
					RunAction(movementAction);

				actionState = ActionState.Walk;
			}
			if (actionState == ActionState.Walk)
			{
				velocity = new CCPoint(direction.X * movementSpeed, direction.Y * movementSpeed);
				if (velocity.X >= 0)
				{
					ScaleX = 1.0f;
					attackBox.Position = new CCPoint(centerToSides, -10);
				}
				else
				{
					//Reposition Attackbox
					attackBox.Position = new CCPoint(-(centerToSides + attackBox.actual.Size.Width), -10);
					ScaleX = -1.0f;
				}
			}
		}



		//collision box factory method
		public BoundingBox createBoundingBoxWithOrigin(CCPoint origin, CCSize size)
		{

			BoundingBox boundingBox = new BoundingBox();

			boundingBox.original.Origin = origin;
			boundingBox.original.Size = size;

			boundingBox.actual.Origin = Position + boundingBox.original.Origin;
			boundingBox.actual.Size = size;

			return boundingBox;
		}

		public void transformBoxes()
		{
			hitBox.actual.Origin = Position + new CCPoint(hitBox.original.Origin.X * ScaleX, hitBox.original.Origin.Y * ScaleY);  //  ccpAdd(position_, ccp(_hitBox.original.origin.x * scaleX_, _hitBox.original.origin.y * scaleY_));
			hitBox.actual.Size = new CCSize(hitBox.original.Size.Width * ScaleX, hitBox.original.Size.Height * ScaleY);  // CGSizeMake(_hitBox.original.size.width * scaleX_, _hitBox.original.size.height * scaleY_);
			attackBox.actual.Origin = Position + new CCPoint(attackBox.original.Origin.X * ScaleX, attackBox.original.Origin.Y * ScaleY);   //  ccpAdd(position_, ccp(_attackBox.original.origin.x * scaleX_, _attackBox.original.origin.y * scaleY_));
			attackBox.actual.Size = new CCSize(attackBox.original.Size.Width * ScaleX, attackBox.original.Size.Height * ScaleY);     // CGSizeMake(_attackBox.original.size.width * scaleX_, _attackBox.original.size.height * scaleY_);
		}


	}
}
