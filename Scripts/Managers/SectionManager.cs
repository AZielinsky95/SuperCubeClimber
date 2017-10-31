using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    private static SectionManager m_Instance;
    public static SectionManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject obj = new GameObject("SectionManager");
                m_Instance = obj.AddComponent<SectionManager>();
                DontDestroyOnLoad(obj);
            }

            return m_Instance;
        }
    }

    [SerializeField] private List<Section> m_OpenSections = new List<Section>();
    [SerializeField] private List<Section> m_ClosedSections = new List<Section>();
    [SerializeField] private Section m_StartSection;

    private Transform m_CurrentAnchor;

    void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
        }

        m_Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        m_CurrentAnchor = m_StartSection.GetAnchor;
    }

    private void CheckInvisibleSections()
    {
        Section temp = m_ClosedSections[0];

        if (!IsVisibleToCamera(temp.GetAnchor))
        {
            Debug.Log("Section Invisible" + temp.name);
            temp.SpawnTriggerEnabled = true;
            m_OpenSections.Add(temp);
            m_ClosedSections.Remove(temp);
            temp.gameObject.SetActive(false);
        }
    }

    public void SpawnSection()
    {
        // Check to see if we can disable previous sections
        if (m_ClosedSections.Count > 0)
        {
            CheckInvisibleSections();
        }
        // Get a random section
        int rand = Random.Range(0, m_OpenSections.Count - 1);
        //Activate new Section!\
        m_OpenSections[rand].gameObject.SetActive(true);
        //If we didn't set the color of the section -> set the correct color
        if (!m_OpenSections[rand].ColorSet)
        {
            ColorManager.Instance.SetSectionColor(m_OpenSections[rand]);
        }
        //Store current anchors position
        Vector3 temp = m_CurrentAnchor.transform.position;
        //Set position to Current anchor!
        m_OpenSections[rand].gameObject.transform.position = new Vector3(temp.x, temp.y,0);
        //Active Diamonds & moving Platforms
        m_OpenSections[rand].ResetSection();
        //New Current Anchor is our newly spawned sections anchor
        m_CurrentAnchor = m_OpenSections[rand].GetAnchor;
        //Add new section to closed sections
        m_ClosedSections.Add(m_OpenSections[rand]);
        //Remove section from open sections 
        m_OpenSections.RemoveAt(rand);
        
    }

    private bool IsVisibleToCamera(Transform s)
    {
        Vector3 visTest = Camera.main.WorldToViewportPoint(s.position);
        return visTest.y > 0;
    }

    public void ResetSections()
    {
        //RESET START SECTION COLORS
        if (!m_StartSection.ColorSet)
        {
            ColorManager.Instance.SetSectionColor(m_StartSection);
        }

      //  m_StartSection.gameObject.SetActive(true);

        //Move all sections to OpenSections!
        foreach (Section section in m_ClosedSections)
        {
            m_OpenSections.Add(section);
        }

        //Clear Closed sections!
        m_ClosedSections.Clear();

        //Disable all Sections!
        foreach (Section section in m_OpenSections)
        {
            section.SpawnTriggerEnabled = true;
            section.gameObject.SetActive(false);
        }

        //Reset anchor to start anchor
        m_CurrentAnchor = m_StartSection.GetAnchor;
    }
}
 