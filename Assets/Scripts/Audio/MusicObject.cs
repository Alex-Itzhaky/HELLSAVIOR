using UnityEngine;

public class MusicObject : MonoBehaviour
{
    private void OnDestroy()
    {
        SoundManager.Instance.RemoveMusicSourceFromList(gameObject);
    }
}
