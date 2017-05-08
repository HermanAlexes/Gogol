using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using Gogol.Abstract;
using Gogol.Entities;
using Microsoft.Ajax.Utilities;
using System.Drawing;
using System.IO;

namespace Gogol.Concrete
{
    public class BooksRepository : IBookRepository
    {
        private GogolStoreEntities _entitiesContext;

        public BooksRepository()
        {
            _entitiesContext = new GogolStoreEntities();
        }

        public IEnumerable<BookDto> Books
        {
            get
            {
                try
                {
                    IEnumerable<BookDto> books;
                    using (_entitiesContext = new GogolStoreEntities())
                    {
                        var booksPocoList = _entitiesContext.Books;

                        books = ConvertListPocoBookToListDtoBook(booksPocoList);
                    }
                    return books;
                }
                catch (EntityException e)
                {
                    Console.WriteLine("{0}, Inner: {1}, StackTrace: {2} ",
                        e.Message, e.InnerException, e.StackTrace);

                    throw;
                }

            }
        }

        public IEnumerable<AuthorDto> Authors
        {
            get
            {
                List<AuthorDto> authors = new List<AuthorDto>();

                foreach (Author auhtor in _entitiesContext.Authors)
                {
                    var authorDto = new AuthorDto
                    {
                        Id = auhtor.Id,
                        Name = auhtor.Name
                    };

                    authors.Add(authorDto);
                }

                return authors;
            }
        }

        public IEnumerable<PublisherDto> Publishers
        {
            get
            {
                List<PublisherDto> publishers = new List<PublisherDto>();

                foreach (Publisher publisher in _entitiesContext.Publishers)
                {
                    var publisherDto = new PublisherDto
                    {
                        Id = publisher.Id,
                        Name = publisher.Name,
                        Country = publisher.Country

                    };

                    publishers.Add(publisherDto);
                }

                return publishers;
            }
        }

        public IEnumerable<CategoryDto> Categories
        {
            get
            {
                List<CategoryDto> categoriesList = new List<CategoryDto>();

                foreach (Category category in _entitiesContext.Categories)
                {
                    var categoryDto = new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name
                    };

                    categoriesList.Add(categoryDto);
                }

                return categoriesList;
            }
        }

        public IEnumerable<BookDto> GetFilteredBooks(FilterRequest filters)
        {
            using (_entitiesContext = new GogolStoreEntities())
            {
                List<Book> filteredBooks = null;

                if (filters.authors != null)
                {
                    if (filters.authors.Count != 0)
                    {
                        filteredBooks =
                            _entitiesContext.Books.Where(
                                x => filters.authors.Contains(x.Author.Name)).ToList();
                    }
                }

                if (filters.publishers != null)
                {
                    if (filters.publishers.Count != 0)
                    {
                        filteredBooks = filteredBooks != null
                            ? filteredBooks.Where(
                                x => filters.publishers.Contains(x.Publisher.Name)).ToList()
                            : _entitiesContext.Books.Where(
                                x => filters.publishers.Contains(x.Publisher.Name)).ToList();
                    }
                }

                if (filters.categories != null)
                {
                    if (filters.categories.Count != 0)
                    {
                        filteredBooks = filteredBooks != null
                            ? filteredBooks.Where(
                                x => filters.categories.Contains(x.Category.Name)).ToList()
                            : _entitiesContext.Books.Where(
                                x => filters.categories.Contains(x.Category.Name)).ToList();
                    }
                }

                return ConvertListPocoBookToListDtoBook(filteredBooks);
            }
        }

        public void SaveBook(BookDto editCandidate)
        {
            try
            {
                CheckOrAddSubEntities(editCandidate);

                Book dbEntry = null;
                if (editCandidate.Id != null)
                {
                    dbEntry = _entitiesContext.Books.Find(editCandidate.Id);
                }

                if (dbEntry != null)
                {
                    ConvertDtoBookToPocoBook(dbEntry, editCandidate);
                }
                else
                {
                    Book book = new Book();
                    ConvertDtoBookToPocoBook(book, editCandidate);
                    _entitiesContext.Books.Add(book);
                }

                _entitiesContext.SaveChanges();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public BookDto DeleteBookById(int bookId)
        {
            Book deleteCandidate = _entitiesContext.Books.Find(bookId);

            BookDto result = ConvertPocoBookToDtoBook(deleteCandidate);

            if (deleteCandidate != null)
            {
                _entitiesContext.Books.Remove(deleteCandidate);

                _entitiesContext.SaveChanges();
            }

            return result;
        }

        private void CheckOrAddSubEntities(BookDto editCandidate)
        {

            var publisher = _entitiesContext.Publishers.FirstOrDefault(r => r.Name == editCandidate.Publisher);

            if (publisher == null)
            {
                AddNewPublisher(editCandidate.Publisher);
            }

            var author = _entitiesContext.Authors.FirstOrDefault(r => r.Name == editCandidate.Author);

            if (author == null)
            {
                AddNewAuthor(editCandidate.Author);
            }

            var category = _entitiesContext.Categories.FirstOrDefault(r => r.Name == editCandidate.Category);

            if (category == null)
            {
                AddNewCategory(editCandidate.Category);
            }

            _entitiesContext.SaveChanges();

        }

        private BookDto ConvertPocoBookToDtoBook(Book bookPoco)
        {
            BookDto bookDto = new BookDto();

            bookDto.Id = bookPoco.Id;
            bookDto.Name = bookPoco.Name;
            bookDto.Category = bookPoco.Category.Name;
            bookDto.Description = bookPoco.Description;
            bookDto.Price = bookPoco.Price;
            bookDto.ISBN = bookPoco.ISBN;
            bookDto.Author = bookPoco.Author.Name;
            bookDto.PublishDate = bookPoco.PublishDate;
            bookDto.Publisher = bookPoco.Publisher.Name;
            bookDto.Illustrator = bookPoco.Illustrator;
            bookDto.Language = bookPoco.Language;
            bookDto.Size = bookPoco.Size;
            bookDto.Weight = bookPoco.Weight;
            bookDto.Series = bookPoco.Series;
            return bookDto;
        }

        private IEnumerable<BookDto> ConvertListPocoBookToListDtoBook(IEnumerable<Book> bookList)
        {
            List<BookDto> booksList = new List<BookDto>();

            foreach (Book book in bookList)
            {
                var bookDto = ConvertPocoBookToDtoBook(book);

                using (_entitiesContext = new GogolStoreEntities())
                {
                    IQueryable<byte[]> photos = _entitiesContext
                        .BooksPhotoes
                        .Where(x => x.BOOK_ID == bookDto.ISBN).Select(x => x.PHOTO);

                    bookDto.Photos = new List<String>();

                    foreach (var photo in photos)
                    {
                        bookDto.Photos.Add(Convert.ToBase64String(photo));
                    }
                }

                booksList.Add(bookDto);
            }

            return booksList;
        }

        private void ConvertDtoBookToPocoBook(Book bookPoco, BookDto bookDto)
        {
            bookPoco.Name = bookDto.Name;
            bookPoco.Description = bookDto.Description;
            bookPoco.PublishDate = bookDto.PublishDate;
            bookPoco.Price = bookDto.Price;
            bookPoco.PublisherId = (_entitiesContext.Publishers.First(r => r.Name == bookDto.Publisher)).Id;
            bookPoco.AuthorId = (_entitiesContext.Authors.First(r => r.Name == bookDto.Author)).Id;
            bookPoco.CategoryId = (_entitiesContext.Categories.First(r => r.Name == bookDto.Category)).Id;

            // for migration purposes 
            bookPoco.ISBN = bookDto.ISBN;
            bookPoco.Illustrator = bookDto.Illustrator;
            bookPoco.Language = bookDto.Language;
            bookPoco.Series = bookDto.Series;
            bookPoco.Size = bookDto.Size;
            bookPoco.Weight = bookDto.Weight;
        }

        private void AddNewAuthor(string name)
        {
            _entitiesContext.Authors.Add(new Author { Name = name });
        }

        private void AddNewPublisher(string name)
        {
            var dedicatedContext = new GogolStoreEntities();

            var addedPublisher = dedicatedContext.Publishers.Add(new Publisher { Name = name, Country = "Ukraine" });//TODO: Country hardcode - Temp

            dedicatedContext.SaveChanges();
        }

        private void AddNewCategory(string name)
        {
            _entitiesContext.Categories.Add(new Category { Name = name });
        }
    }
}
