using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStance : MonoBehaviour {

    private Sprite AttackSprite;
    private Sprite DefenseSprite;
    private Sprite CurrentSprite;
    private SpriteRenderer SpriteRenderer;
    private  bool touch_flag;
    private void Start()
    {
        AttackSprite = Resources.Load("Swords",typeof(Sprite)) as Sprite;
        DefenseSprite = Resources.Load("Shield", typeof(Sprite)) as Sprite;
        SpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            if (hit.collider.gameObject != null)
            {
                switch (touch.phase)
                {

                    case TouchPhase.Began:
                        touch_flag = true;
                        if (hit.collider.gameObject == gameObject && touch_flag == true)
                        {
                            CurrentSprite = SpriteRenderer.sprite;
                            if (CurrentSprite == AttackSprite)
                            {

                                SpriteRenderer.sprite = DefenseSprite;
                            }
                            else if (CurrentSprite == DefenseSprite)
                            {
                                SpriteRenderer.sprite = AttackSprite;
                            }

                        }
                        touch_flag = false;
                        break;

                    case TouchPhase.Ended:
                        touch_flag = false;
                        break;
                }
            }
        }   
    }
}
