using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager
{
    private readonly List<Reaction> reactions = new List<Reaction>();

    public List<Reaction> Reactions { get => reactions; }

    public ReactionManager(List<Chemical> chemicals)
    {
        TextAsset reactionsAsset = (TextAsset)Resources.Load("Reactions", typeof(TextAsset));
        string reactionsText = reactionsAsset.text;
        List<string> lines = new List<string>();

        //Seperating text line by line
        string bufferText = "";
        foreach (char _ in reactionsText)
        {
            if (_ == '\n')
            {
                lines.Add(bufferText);
                bufferText = "";
            }
            else
            {
                bufferText += _.ToString();
            }
        }
        lines.Add(bufferText);

        foreach (string line in lines)
        {
            string[] reaction = line.Split('#');
            reactions.Add(new Reaction(reaction[0], reaction[1], reaction[2], chemicals));
        }

    }


}
