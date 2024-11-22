using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace GameMain
{
    public class MyDialogueActor : MonoBehaviour, IDialogueActor
    {
        public Texture2D portrait { get; set; }
        public Sprite portraitSprite { get; set; }
        public Color dialogueColor { get; set; }
        public Vector3 dialoguePosition { get; set; }

        string IDialogueActor.name
        {
            get => _actorName;
        }

        public string _actorName;
    }
}