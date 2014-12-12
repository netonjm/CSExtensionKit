using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CocosSharp
{

	public static class CCSizeEx
	{
		public static CCPoint ToPoint(this CCSize sender)
		{
			return new CCPoint(sender.Width, sender.Height);
		}
	}

	public static class CCPointExHelper
	{

		public static CCPoint GetRandomPosition(CCSize wSize, float minX, float maxX, float minY, float maxY)
		{

			return new CCPoint(CCRandom.GetRandomFloat(minX, maxX), CCRandom.GetRandomFloat(minY, maxY));
		}

		public static Vector2 ToVector2(CCPoint point)
		{
			return new Vector2(point.X, point.Y);

		}

		public static bool IsPointInCircle(CCPoint point, CCPoint center, float radius)
		{
			float dx = (point.X - center.X);
			float dy = (point.Y - center.Y);
			return (radius >= Math.Sqrt((dx * dx) + (dy * dy)));
		}


		/**
		* Calculates difference of two points.
		*
		* @return CCPoint
		*/
		public static CCPoint ccpSub(CCPoint v1, CCPoint v2)
		{

			return new CCPoint(v1.X - v2.X, v1.Y - v2.Y);
		}

		/**
	   * Calculates dot product of two points.
	   *
	   * @return float
	   */
		public static float ccpDot(CCPoint v1, CCPoint v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}


		/**
		* Calculates the square length of a CCPoint (not calling sqrt() )
		*
		* @return float
		*/
		public static float ccpLengthSQ(CCPoint v)
		{
			return ccpDot(v, v);
		}

		/** Calculates distance between point and origin
		 @return CGFloat
		 @since v0.7.2
		 */
		public static float ccpLength(CCPoint v)
		{
			return (float)Math.Sqrt(ccpLengthSQ(v));
		}

		/**
		* Calculates the distance between two points
		*
		* @return float
		*/
		public static float ccpDistance(CCPoint v1, CCPoint v2)
		{
			return ccpLength(ccpSub(v1, v2));
		}

		public static CCPoint ccpAdd(CCPoint v1, CCPoint v2)
		{
			return new CCPoint(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static CCPoint ccpMult(CCPoint v, float s)
		{
			return new CCPoint(v.X * s, v.Y * s);
		}


		public static CCPoint ccpMidpoint(CCPoint v1, CCPoint v2)
		{
			return ccpMult(ccpAdd(v1, v2), 0.5f);
		}

		public static CCPoint b2Mul(CCPoint ant, int p)
		{
			return new CCPoint(ant.X * p, ant.Y * p);
		}

		public static CCPoint ccpRPerp(CCPoint a)
		{
			return new CCPoint(a.Y, -a.X);
		}

		public static CCPoint ccpClamp(CCPoint p, CCPoint min_inclusive, CCPoint max_inclusive)
		{
			return new CCPoint(clampf(p.X, min_inclusive.X, max_inclusive.X), clampf(p.Y, min_inclusive.Y, max_inclusive.Y));
		}

		public static float clampf(float value, float min_inclusive, float max_inclusive)
		{
			if (min_inclusive > max_inclusive)
			{
				CC_SWAP(ref min_inclusive, ref max_inclusive);
			}
			return value < min_inclusive ? min_inclusive : value < max_inclusive ? value : max_inclusive;
		}

		public static void CC_SWAP(ref int x, ref int y)
		{
			int temp = x;
			x = y;
			y = temp;
		}

		public static void CC_SWAP(ref float x, ref float y)
		{
			float temp = x;
			x = y;
			y = temp;
		}

		public static CCPoint GetRandomPosition(CCSize size)
		{

			return new CCPoint(
				CCRandom.GetRandomInt(0, (int)size.Width),
				CCRandom.GetRandomInt(0, (int)size.Height)
			);

		}

		public static CCPoint GetRandomPositionBoundingX(CCSize size, CCRect BoundingBox, int separation)
		{
			return GetRandomPositionBoundingX(size, BoundingBox, 0, separation);
		}

		public static CCPoint GetRandomPositionBoundingX(CCSize size, CCRect BoundingBox, int minY, int separation)
		{

			return new CCPoint(
				CCRandom.GetRandomInt(minY, (int)size.Height),
				CCRandom.GetRandomInt(minY, (int)size.Height)
			);

		}


		public static CCPoint GetAleatoryPosition(CCSize wSize)
		{

			return new CCPoint(
			   CCRandom.GetRandomFloat(0f, wSize.Width),
			 CCRandom.GetRandomFloat(0, wSize.Height)
			   );

		}

		public static CCPoint GetAleatoryPositionBoundingX(CCRect BoundingBox, int separation, CCSize wSize)
		{
			return GetAleatoryPositionBoundingX(BoundingBox, 0, separation, wSize);
		}

		public static CCPoint GetAleatoryPositionBoundingX(CCRect BoundingBox, int minY, int separation, CCSize wSize)
		{
			return new CCPoint(
				 CCRandom.GetRandomFloat(minY,
			   wSize.Height),
			   CCRandom.GetRandomFloat(minY,
			   wSize.Height)
			   );
		}


		//new CCPoint(selected.BoundingBox.Size.Width + random, FirstLevelLayer.GetAleatoryNumber(100, (int)CCDirector.SharedDirector.WinSize.Height));



	}
}
