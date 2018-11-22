 using UnityEngine;
 using System.Collections;
 
 // from https://answers.unity.com/questions/799429/transformfindstring-no-longer-finds-grandchild.html
 public static class TransformFindDeepChildExtension
 {
     //Breadth-first search
     public static Transform FindDeepChild(this Transform aParent, string aName)
     {
         var result = aParent.Find(aName);
         if (result != null)
             return result;
         foreach(Transform child in aParent)
         {
             result = child.FindDeepChild(aName);
             if (result != null)
                 return result;
         }
         return null;
     }
 }
 