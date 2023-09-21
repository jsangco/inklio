namespace Inklio.Api.Client;

public static class SampleComments
{
    public static string[] AskCommentBodies = new string[]
    {
        "I can't wait to see what the deliveries!",
        "Eagerly anticipating the delivery.",
        "Excited to see an artistic vision come to life!",
        "Looking forward to the amazing art that will emerge.",
        "Anticipating the beauty that'll be captured on canvas.",
        "Can't wait to witness the creative magic!",
        "I'm excited to see the art.",
        "Looking forward to the this one.",
        "Anticipation the results of this!",
        "Can't wait to see the delivery.",
        "I'm awaiting the delivery for this.",
        "Can't wait to see the artistry that emerges.",
        "Looking forward to the delivery.",
        "Anticipating the art that will come from this.",
        "Can't wait to see this one!",
        "I second this.",
        "I'm excited for this!",
        "Can't wait to see what people do with this.",
        "Yes!",
        "Can't wait to see art that comes from this.",
        "Anticipating the result of this.",
    };

    public static IEnumerable<CommentCreate> AskCommentCreate => AskCommentBodies.Select(b => new CommentCreate(b));

    public static string[] DeliveryCommentBodies = new string[]
    {
        "Wow, your talent shines through in this artwork!",
        "This is fantastic! Your skills are impressive.",
        "thanks!",
        "I'm loving the creativity in this piece. Great job!",
        "Well done",
        "Incredible work! Your art amazes me.",
        "Great!",
        "Bravo! Your artistic expression is captivating.",
        "Such a beautiful and unique delivery. Well done!",
        "thx",
        "This is a masterpiece! You're a talented artist.",
        "I'm blown away. Keep up the great work!",
        "Absolutely stunning!",
        "cool",
        "What a delightful piece!",
        "You have a gift.",
        "This is pure artistry at its finest. Well done!",
        "I'm impressed by your artistic flair.",
        "THANKS!",
        "Your art brightens my day. Keep up the amazing work!",
        "This is a masterpiece",
        "Thank you.",
        "I can't get enough of this incredible artwork.",
        "Your talent is awe-inspiring. Thanks for sharing this beauty.",
        "nice",
        "This artwork is amazing.",
        "splendid",
        "I'm genuinely impressed by this delivery.",
        "You're a true artist, and it shows.",
        "This is so good. Keep up the great work!",
        "Nice!",
        "Not bad",
        "You've captured the essence of creativity in this piece.",
        "Awesome!",
        "wow",
        "What a wonderful creation! Thanks for sharing your talent.",
        "You've created something great here. Well done!",
        "Nice I hope you will do more.",
        "beyond my expectations",
        "This drawing is a masterpiece in the making. Keep doing more!",
    };

    public static IEnumerable<CommentCreate> DeliveryCommentCreate => DeliveryCommentBodies.Select(b => new CommentCreate(b));
}