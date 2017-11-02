using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bookshelf))]
public class BookSorterEditor : Editor
{
    private Bookshelf bookSorter;

    private BoxCollider2D[] bookColliders;
    private List<SpriteRenderer> books;
    private float[] bookWidths;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bookSorter = (Bookshelf)target;

        if (bookSorter.books.Count != 10)
        {
            MonoBehaviour.print("Populate all books");
        }              

        if (GUILayout.Button("Sort books"))
        {
            SortBooks();
        }
    }

    public void SortBooks()
    {
        books = bookSorter.books;
        bookColliders = new BoxCollider2D[books.Count];
        bookWidths = new float[books.Count];

        float totalWidthOfBooks = 0;

        // Populate colliders array
        for (int i = 0; i < books.Count; i++)
        {
            bookColliders[i] = books[i].GetComponent<BoxCollider2D>();
        }

        // Populate widths array and find total width of all books
        for (int i = 0; i < books.Count; i++)
        {
            bookWidths[i] = bookColliders[i].bounds.size.x;
            totalWidthOfBooks += bookWidths[i];
        }

        // Start position of the first book is negative half width of all books (so that books are centered before more precise position is set in editor)
        Vector3 bookPosition = new Vector3(-totalWidthOfBooks / 2f + bookSorter.transform.position.x, bookSorter.transform.position.y, 0);
        books[0].transform.position = bookPosition;

        // Position all other books
        for (int i = 1; i < books.Count; i++)
        {
            // Half width of the previous book + its offset.x + half width of the current book - its offset.x
            bookPosition.x += bookWidths[i - 1] / 2 + bookColliders[i - 1].offset.x + bookWidths[i] / 2 - bookColliders[i].offset.x;
            books[i].transform.position = bookPosition;
        }
    }
}
