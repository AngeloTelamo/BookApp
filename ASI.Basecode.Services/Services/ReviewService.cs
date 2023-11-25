using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.IO;
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
}
