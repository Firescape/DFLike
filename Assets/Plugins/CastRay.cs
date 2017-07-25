using UnityEngine;
using System;

public class CastRay : object
{
    public static GridHit castRay(float range, Point mapSize, Func<Point, Block> getBlock)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var planes = new Plane[3];
        var point = 0f;
        var closestPoint = -1f;
        Vector3 hitPoint;
        var planeHit = -1;
        var side = 0;

        var step = new Vector3(Mathf.Sign(ray.direction.x), Mathf.Sign(ray.direction.y), Mathf.Sign(ray.direction.z));

        var currentBlock = new Vector3(Mathf.FloorToInt(ray.origin.x), Mathf.FloorToInt(ray.origin.y),
            Mathf.FloorToInt(ray.origin.z));
        var blockSides = new Vector3(0, 0, 0);

        var loopSentinel = 256;

        for (var i = 0; i < loopSentinel; i++)
        {
            blockSides = new Vector3(currentBlock.x + Mathf.Clamp01(step.x), currentBlock.y + Mathf.Clamp01(step.y),
                currentBlock.z + Mathf.Clamp01(step.z));

            planes[0] = new Plane(new Vector3(0, blockSides.y, 0), new Vector3(1, blockSides.y, 0),
                new Vector3(0, blockSides.y, 1));
            planes[1] = new Plane(new Vector3(0, 0, blockSides.z), new Vector3(1, 0, blockSides.z),
                new Vector3(0, 1, blockSides.z));
            planes[2] = new Plane(new Vector3(blockSides.x, 0, 0), new Vector3(blockSides.x, 1, 0),
                new Vector3(blockSides.x, 0, 1));

            for (var i2 = 0; i2 < 3; i2++)
            {
                planes[i2].Raycast(ray, out point);
                if (closestPoint == -1 && planes[i2].Raycast(ray, out point))
                {
                    closestPoint = point;
                    planeHit = i2;
                }
                else if (point < closestPoint && point != 0)
                {
                    closestPoint = point;
                    planeHit = i2;
                }
            }

            hitPoint = ray.GetPoint(closestPoint);
            if (planeHit == 2)
            {
                currentBlock.x = currentBlock.x + step.x;
                if (step.x == 1)
                    side = 3; //left
                else
                    side = 2; //right
            }
            else if (planeHit == 0)
            {
                currentBlock.y = currentBlock.y + step.y;
                if (step.y == 1)
                    side = 5; //bottom
                else
                    side = 0; //top
            }
            else if (planeHit == 1)
            {
                currentBlock.z = currentBlock.z + step.z;
                if (step.z == 1)
                    side = 4; //back
                else
                    side = 1; //front
            }

            if (Mathf.Abs(closestPoint) > range)
                return null;
            else if (G.withinLim(Point.ToPoint(currentBlock), mapSize))
                if (getBlock(Point.ToPoint(currentBlock)).getType() > 0)
                    return new GridHit(Point.ToPoint(currentBlock), side);
            closestPoint = -1;
        }
        throw new SystemException("this throw statement should not have been reached");
    }
}