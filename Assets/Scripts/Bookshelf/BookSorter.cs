﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSorter : MonoBehaviour
{
    private static Vector3 normalScale = new Vector3(1, 1, 1);
    private static Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1);

    private Transform myTransform;

    public List<SpriteRenderer> books;
    [SerializeField]
    private List<SpriteRenderer> orderedBooks;
    private BoxCollider2D[] bookColliders;
    private float[] bookWidths;
    float totalWidthOfBooks;
        
    private SpriteRenderer selectedBook;
    private bool isPuzzleSolved;

    public SpriteRenderer SelectedBook
    {
        get
        {
            return selectedBook;
        }
        set
        {
            if (isPuzzleSolved)
            {
                // Puzzle is solved, so books cannot be selected or swapped
                return;
            }

            AudioManager.Instance.PlayBookSelectedAudio();

            if (selectedBook == null)
            {
                // First selection
                selectedBook = value;
                ChangeBookSelection(selectedBook, true);
            }
            else if (selectedBook == value)
            {
                // Deselect
                ChangeBookSelection(selectedBook, false);
                selectedBook = null;
            }
            else
            {
                // Swap books
                ChangeBookSelection(selectedBook, false);
                SwapBookPlaces(selectedBook, value);
                SortBooks();
                CheckBookOrder();
                selectedBook = null;
            }
        }
    }
    public bool IsPuzzleSolved
    {
        get
        {
            return isPuzzleSolved;
        }
    }

    private void Awake()
    {
        myTransform = transform;

        if (books.Count != 10)
        {
            print("Populate all books");
            return;
        }

        bookColliders = new BoxCollider2D[books.Count];
        bookWidths = new float[books.Count];

        for (int i = 0; i < books.Count; i++)
        {
            // Populate colliders array
            bookColliders[i] = books[i].GetComponent<BoxCollider2D>();
            
            // Populate widths array 
            bookWidths[i] = bookColliders[i].bounds.size.x;

            // Add current book's width to total width
            totalWidthOfBooks += bookWidths[i];
        }
    }

    private void SortBooks()
    {
        // Start position of the first book is negative half width of all books (plus position.x) so that they are always centered
        Vector3 bookPosition = new Vector3(-totalWidthOfBooks / 2f + myTransform.position.x, myTransform.position.y, 0);
        books[0].transform.position = bookPosition;

        // Position all other books
        for (int i = 1; i < books.Count; i++)
        {
            // Half width of the previous book + its offset.x + half width of the current book - its offset.x
            bookPosition.x += bookWidths[i - 1] / 2 + bookColliders[i - 1].offset.x + bookWidths[i] / 2 - bookColliders[i].offset.x;
            books[i].transform.position = bookPosition;
        }
    }

    private void ChangeBookSelection(SpriteRenderer book, bool isSelected)
    {
        book.transform.localScale = isSelected ? selectedScale : normalScale;
        selectedBook.sortingOrder = isSelected ? 1 : 0;
    }

    private void SwapBookPlaces(SpriteRenderer firstBook, SpriteRenderer secondBook)
    {
        int firstIndex = books.IndexOf(firstBook);
        int secondIndex = books.IndexOf(secondBook);

        books[secondIndex] = firstBook;
        books[firstIndex] = secondBook;

        BoxCollider2D tempCollider = bookColliders[firstIndex];
        bookColliders[firstIndex] = bookColliders[secondIndex];
        bookColliders[secondIndex] = tempCollider;
        
        float tempWidth = bookWidths[firstIndex];
        bookWidths[firstIndex] = bookWidths[secondIndex];
        bookWidths[secondIndex] = tempWidth;
    }

    private void CheckBookOrder()
    {
        for (int i = 0; i < books.Count; i++)
        {
            if (books[i] != orderedBooks[i])
            {
                return;
            }
        }

        isPuzzleSolved = true;
        AudioManager.Instance.PlayPuzzleSolvedAudio();
        // TODO give player something
    }
}
