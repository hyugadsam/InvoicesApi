﻿using AutoMapper;
using Dtos.Common;
using Dtos.Request;
using System.Collections.Generic;
using System.Linq;
using WebSite.Models.Authors;
using WebSite.Models.Books;

namespace WebSite.Utility
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            #region Authors
            CreateMap<FullAuthorDto, AuthorDto>();
            CreateMap<AuthorDto, SelectAuthorModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(a => a));
            //CreateMap<AuthorDto, Author>();
            //CreateMap<Author, FullAuthorDto>()
            //    .ForMember(dest => dest.BooksList, opt => opt.MapFrom(MapAuthorToFullDto));
            //CreateMap<FullAuthorDto, Author>()
            //    .ForMember(dest => dest.AuthorBooks, opt => opt.MapFrom(MapFullAuthorDtoToAuthor));

            #endregion

            #region Books
            CreateMap<FullBookDto, UpdateBookModel>();
            CreateMap<UpdateBookModel, UpdateBookRequest>()
                .ForMember(dest => dest.AuthorsList, opt => opt.MapFrom(model => model.AuthorsId));
            //CreateMap<Book, BookDto>();
            //CreateMap<Book, FullBookDto>()
            //    .ForMember(dest => dest.AuthorsList, opt => opt.MapFrom(MapBookToBookDto));
            //CreateMap<FullBookDto, Book>()
            //    .ForMember(dest => dest.AuthorBooks, opt => opt.MapFrom(MapFullBookDtoToBook));
            #endregion

            #region Clients
            //CreateMap<ClientDto, Client>();
            //CreateMap<Client, ClientDto>();
            //CreateMap<Client, FullClientDto>()
            //    .ForMember(dest => dest.BorrowedBooks, opt => opt.MapFrom(MapAuthorToFullDto));

            #endregion

        }

        #region Methods

        //private List<int> MapAuthorListFromChecks(UpdateBookModel model, UpdateBookRequest request)
        //{
        //    var list = new List<int>();
        //    if (model == null || model.AuthorList == null || model.AuthorList?.Count == 0)
        //        return list;

        //    list = model.AuthorList.Where(a => a.Check).Select(at => at.AuthorId).ToList();

        //    return list;
        //}

        //private List<BookDto> MapAuthorToFullDto(Client client, FullClientDto dto)
        //{
        //    var lista = new List<BookDto>();

        //    if (client == null || client.BorrowedBooks == null || client.BorrowedBooks?.Count == 0)
        //        return lista;

        //    foreach (var item in client.BorrowedBooks)
        //    {
        //        BookDto libro = new BookDto
        //        {
        //            BookId = item.BookId,
        //            CreateDate = item.CreateDate,
        //            Title = item.Title
        //        };
        //        lista.Add(libro);
        //    }

        //    return lista;
        //}

        //private List<AuthorBook> MapFullBookDtoToBook(FullBookDto dto, Book book)
        //{
        //    var list = new List<AuthorBook>();
        //    if (dto == null || dto.AuthorsList == null || dto.AuthorsList?.Count == 0)
        //        return list;

        //    foreach (var item in dto.AuthorsList)
        //    {
        //        list.Add(new AuthorBook
        //        {
        //            AuthorId = item.AuthorId,
        //            BookId = dto.BookId,
        //        });
        //    }
        //    return list;
        //}

        //private List<AuthorDto> MapBookToBookDto(Book book, FullBookDto dto)
        //{
        //    var lista = new List<AuthorDto>();
        //    if (book == null || book.AuthorBooks == null || book.AuthorBooks?.Count == 0)
        //        return lista;

        //    foreach (var item in book.AuthorBooks)
        //    {
        //        lista.Add(new AuthorDto
        //        {
        //            AuthorId = item.AuthorId,
        //            Name = item.Author?.Name
        //        });
        //    }
        //    return lista;
        //}

        //private List<AuthorBook> MapFullAuthorDtoToAuthor(FullAuthorDto dto, Author author)
        //{
        //    var lista = new List<AuthorBook>();
        //    if (dto == null || dto.BooksList == null || dto.BooksList?.Count == 0)
        //        return lista;

        //    foreach (var item in dto.BooksList)
        //    {
        //        var book = new AuthorBook
        //        {
        //            AuthorId = author.AuthorId,
        //            BookId = item.BookId
        //        };
        //        lista.Add(book);
        //    }

        //    return lista;
        //}

        //private List<BookDto> MapAuthorToFullDto(Author author, FullAuthorDto dto)
        //{
        //    var lista = new List<BookDto>();

        //    if (author == null || author.AuthorBooks == null || author.AuthorBooks?.Count == 0)
        //        return lista;

        //    foreach (var item in author.AuthorBooks)
        //    {
        //        BookDto libro = new BookDto
        //        {
        //            BookId = item.BookId,
        //            CreateDate = item.Book.CreateDate,
        //            Title = item.Book.Title
        //        };
        //        lista.Add(libro);
        //    }

        //    return lista;
        //}


        #endregion

    }
}
