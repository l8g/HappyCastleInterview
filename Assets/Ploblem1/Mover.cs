using System.Collections;

using UnityEngine;

public class Mover : MonoBehaviour
{
    public GameObject objectToMove;
    public Vector3 start;
    public Vector3 end;
    public float duration;
    public bool pingpong;
    public EaseType easeType;

    private Coroutine moveCoroutine;

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            Move(objectToMove, start, end, duration, pingpong, easeType);
        }
    }

    void Start()
    {
        Move(objectToMove, start, end, duration, pingpong, easeType);
    }

    public void Move(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong, EaseType easeType = EaseType.Linear)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveCoroutine(gameObject, begin, end, time, pingpong, easeType));
    }

    private IEnumerator MoveCoroutine(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong, EaseType easeType)
    {
        float elapsedTime = 0;

        while (true)
        {
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / time);

                switch (easeType)
                {
                    case EaseType.EaseIn:
                        t = EaseIn(t);
                        break;
                    case EaseType.EaseOut:
                        t = EaseOut(t);
                        break;
                    case EaseType.EaseInOut:
                        t = EaseInOut(t);
                        break;
                    case EaseType.Linear:
                    default:
                        break;
                }

                gameObject.transform.position = Vector3.Lerp(begin, end, t);
                yield return null;
            }

            if (pingpong)
            {
                elapsedTime = 0;
                Vector3 temp = begin;
                begin = end;
                end = temp;
            }
            else
            {
                yield break;
            }
        }
    }

    private float EaseIn(float t)
    {
        return t * t * t;
    }

    private float EaseOut(float t)
    {
        t = t - 1;
        return t * t * t + 1;
    }

    private float EaseInOut(float t)
    {
        if (t < 0.5f)
        {
            return 4 * t * t * t;
        }
        t = t - 1;
        return 4 * t * t * t + 1;
    }

    public enum EaseType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut
    }
}
