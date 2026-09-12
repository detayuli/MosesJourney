using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    public bool IsPlaced { get; private set; } = false;

    public void Placed()
    {
        IsPlaced = true;
        // audiomanager.Instance.SFXSource.PlayOneShot(audiomanager.Instance.PressSound);
    }
}