using System;
using UnityEngine;
using DG.Tweening;
public class Card : MonoBehaviour
{

    public Colour color;
    
    public void PlayAnimation(Slot targetSlot,float duration,Ease e)
    {

        var rotationVector = new Vector3();
        var currentRotation = transform.rotation.eulerAngles;
        Debug.Log(currentRotation);

        switch (GetMovementDirection(targetSlot.transform))
        {
            case Direction.North:
            {
                rotationVector = new Vector3(currentRotation.x + 180,0,0);
                break;
            }
            case Direction.South:
            {
                rotationVector = new Vector3( currentRotation.x - 180,0,0);
                break;
            }
            case Direction.East:
            {
                rotationVector = new Vector3(0,0,currentRotation.z - 180);
                break;
            }
            case Direction.West:
            {
                rotationVector = new Vector3(0,0,currentRotation.z + 180);
                break;
            }
            case Direction.NorthEast:
            {
                rotationVector = new Vector3(currentRotation.x + 180,0,0);
                break;
            }
            case Direction.NorthWest:
            {
                rotationVector = new Vector3(currentRotation.x + 180,0,0);
                break;
            }
            case Direction.SouthEast:
            {
                rotationVector = new Vector3(currentRotation.x - 180,0,0);
                break;
            }
            case Direction.SouthWest:
            {
                rotationVector = new Vector3(currentRotation.x - 180,0,0);
                break;
            }
        }
        
        Debug.Log(rotationVector);

        
        transform.DOJump(targetSlot.transform.position,2,1,duration).SetEase(e);
        transform.DORotate(rotationVector,duration);
    }
    
    private Direction GetMovementDirection(Transform target)
    {
        var dir = target.position - transform.position;

        var result = dir.x switch
        {
            0 when dir.z > 0 => Direction.North,
            0 when dir.z < 0 => Direction.South,
            > 0 when dir.z == 0 => Direction.East,
            < 0 when dir.z == 0 => Direction.West,
            > 0 when dir.z > 0 => Direction.NorthEast,
            < 0 when dir.z > 0 => Direction.NorthWest,
            > 0 when dir.z < 0 => Direction.SouthEast,
            < 0 when dir.z < 0 => Direction.SouthWest,
            _ => Direction.Unknown
        };
        
        return result;
    }
    
}
