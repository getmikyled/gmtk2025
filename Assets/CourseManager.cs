using UnityEngine;

public class CourseManager : MonoBehaviour
{
    public Course course1;
    public Course course2;
    
    public void DisableAllCourses()
    {
        course1.gameObject.SetActive(false);
        course2.gameObject.SetActive(false);
    }
}
