using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Used on the title screen to make the pattern scroll diagonally
// Idea stolen from Animal Crossing, and I also saw it in Neku Atsume
public class BgScroller : MonoBehaviour
{
	public float offset = 0.25f;
    RawImage Img;

    void Start()
    {
        Img = GetComponent<RawImage>();
    }

	// Update is called once per frame
	void Update () {
        var r = Img.uvRect;
        var offs = offset * Time.deltaTime;
        Img.uvRect = new Rect(r.x + offs, r.y + offs, r.width, r.height);
	}
}
