using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
[SerializeField] private Transform m_Anchor;
[SerializeField] private BoxCollider2D m_SpawnTrigger;
[SerializeField] private MovingPlatform[] m_MovingPlatforms;
[SerializeField] private Diamond m_Diamond;
[SerializeField] private ExtraJump m_ExtraJump;

    private bool m_ColorSet = false;

private bool m_SpawnTriggerEnabled = true;

public bool ColorSet { get { return m_ColorSet; } set { m_ColorSet = value; } }

public Transform GetAnchor { get  { return m_Anchor; } }

public bool SpawnTriggerEnabled {  get { return m_SpawnTriggerEnabled;  } set { m_SpawnTriggerEnabled = value; } }

 public void ResetSection()
 {
        if(m_Diamond!=null)
            m_Diamond.gameObject.SetActive(true);

        if (m_ExtraJump != null)
        {
            m_ExtraJump.gameObject.SetActive(true);
            m_ExtraJump.ChangeColorOverTime();
        }

        if (m_MovingPlatforms != null)
        {
            foreach (MovingPlatform m in m_MovingPlatforms)
            {
                m.ResetPosition();
                m.MoveUpDown();
            }
        }
 }

}
