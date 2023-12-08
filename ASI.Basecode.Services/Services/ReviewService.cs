using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookMasterRepository _bookMasterRepository;

    public ReviewService(IReviewRepository reviewRepository, IBookMasterRepository bookMasterRepository)
    {
        _reviewRepository = reviewRepository;
        _bookMasterRepository = bookMasterRepository;
    }

    public void AddReviews(BookReviewsModel model)
    {
        var review = model.Reviews;
        var master = model.BookMaster;

        var rev = new Review
        {
            ReviewName = review.ReviewName,
            ReviewRatings = review.ReviewRatings,
            ReviewComments = review.ReviewComments,
            ReviewDate = DateTime.Now,
            BookId = model.BookId,
        };

        // Add the review to the database
        _reviewRepository.AddReview(rev);
    }

    public void DeleteReview(int reviewId)
    {
        var reviewsToDelete = _reviewRepository.GetReviews()
                  .Where(x => x.ReviewId == reviewId)
                   .FirstOrDefault();

        if (reviewsToDelete != null)
        {
            _reviewRepository.DeleteReviews(reviewsToDelete);
        }
        else
        {
            throw new InvalidDataException(ASI.Basecode.Resources.Messages.Errors.BookNotExists);
        }
    }
     public ReviewList GetReviewList(ReviewList model)
     {
        ReviewList reviewList = new ReviewList();

        var queryData = _reviewRepository.GetReviews();

        if (model != null && model.Filters != null)
        {
            if (!string.IsNullOrEmpty(model.Filters.SearchTerm))
            {
                queryData = queryData
                    .Where(x => x.BookId.ToString().Contains(model.Filters.SearchTerm) || 
                    x.ReviewName.ToLower().Contains(model.Filters.SearchTerm.ToLower())
                    );
            }
            else if (!string.IsNullOrEmpty(model.Filters.ReviewName))
            {
                queryData = queryData
                    .Where(x => x.ReviewName.ToLower().Contains(model.Filters.ReviewName.ToLower()));
            }
            else if (model.Filters.BookIds != 0)
            {
                queryData = queryData
                    .Where(x => x.BookId == model.Filters.BookIds);
            }
        }

        var reviewData = queryData
         .Select(x => new BookReviewsModel
         {
             Reviews = new ReviewViewModel
             {
                 reviewId = x.ReviewId,
                 ReviewName = x.ReviewName,
                 ReviewComments = x.ReviewComments,
                 ReviewDate = x.ReviewDate,
                 ReviewRatings = x.ReviewRatings
             },
             BookMaster = new BookMasterViewModel
             {
                 BookId = x.BookId,
                 BookTitle = x.Book.BookTitle,
                 BookAuthor = x.Book.BookAuthor,
                 BookGenreName = x.Book.genreMaster.GenreName,
             },
         }).ToList();

        reviewList.ReviewLists = reviewData;

        // Add total review count
        reviewList.TotalReviews = reviewData.Count;

        return reviewList;
    }
}
