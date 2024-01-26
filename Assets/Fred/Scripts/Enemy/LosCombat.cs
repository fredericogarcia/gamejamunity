using UnityEngine;

public class LosCombat : MonoBehaviour
{
    [SerializeField] private float distance;
    private RaycastHit2D _hit;
    private Vector2 _endPosition;
     
    public Collider2D LineOfSight()
    {
        _endPosition = transform.position + transform.localPosition * distance;
        _hit = Physics2D.Linecast(transform.position, _endPosition,
            1 << LayerMask.NameToLayer("Combat"));

        Debug.DrawLine(transform.position, _endPosition, _hit.collider != null ? Color.green : Color.blue);
        return _hit.collider != null ? _hit.collider : null;
    }

    private void Update()
    {
        LineOfSight();
    }
}