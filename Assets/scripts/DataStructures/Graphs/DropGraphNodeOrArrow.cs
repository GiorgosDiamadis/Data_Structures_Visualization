using UnityEngine;
using UnityEngine.EventSystems;

public class DropGraphNodeOrArrow : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject graph_prefab;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject new_node = Instantiate(graph_prefab);

        new_node.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 90);
        new_node.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90);

        
        new_node.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, UnityEngine.Input.mousePosition.z));
        new_node.transform.SetParent(ViewHandler.view.transform,true);
    
        new_node.transform.localScale = Vector3.one;
        new_node.transform.localPosition = new_node.transform.localPosition.With(z: 0);

        new_node.transform.Get_Component_In_Child<TMPro.TextMeshProUGUI>(0, 0).text = (Graphs.node_count++).ToString();

    }
}
