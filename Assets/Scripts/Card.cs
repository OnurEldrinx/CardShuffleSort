using UnityEngine;
using DG.Tweening;
public class Card : MonoBehaviour
{

    public Colour color;
    
    public Tween PlayAnimation(Slot targetSlot,float duration,Ease e,float offset)
    {
        
        var rotationVector = new Vector3();
        var currentRotation = transform.rotation.eulerAngles;

        rotationVector = GetMovementDirection(targetSlot.transform) switch
        {
            Direction.North => new Vector3(currentRotation.x + 180, 0, 0),
            Direction.South => new Vector3(currentRotation.x - 180, 0, 0),
            Direction.East => new Vector3(0, 0, currentRotation.z - 180),
            Direction.West => new Vector3(0, 0, currentRotation.z + 180),
            Direction.NorthEast => new Vector3(currentRotation.x + 180, 0, 0),
            Direction.NorthWest => new Vector3(currentRotation.x + 180, 0, 0),
            Direction.SouthEast => new Vector3(currentRotation.x - 180, 0, 0),
            Direction.SouthWest => new Vector3(currentRotation.x - 180, 0, 0),
            _ => rotationVector
        };

        var position = targetSlot.transform.position;
        var p = new Vector3(position.x, 0 + offset, position.z);
        
        Tween j = transform.DOJump(p,3,1,duration).SetEase(e);
        transform.DORotate(rotationVector, duration).SetEase(e);

        return j;

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
