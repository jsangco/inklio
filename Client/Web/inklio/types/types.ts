export type ODataResponse<T> = {
  context: string | null;
  nextLink: string | null;
  count: number | null;
  value: T[];
  error: any;
}

export type Ask = {
  id: number;
  body: string;
  canComment: boolean;
  canDeliver: boolean;
  canEdit: boolean;
  canFlag: boolean;
  canTag: boolean;
  createdAtUtc: Date;
  createdBy: string;
  editedAtUtc: Date | null;
  editedBy: string | null;
  flagCount: boolean;
  isDeleted: boolean;
  isDelivered: boolean;
  isDeliveryAccepted: boolean;
  isLocked: boolean;
  isNsfw: boolean;
  isNsfl: boolean;
  lockedAtUtc: Date | null;
  saveCount: boolean;
  title: string;
  upvoteCount: number;
  viewCount: number;
  tags: Tag[];
  deliveries: Delivery[];
  comments: Comment[];
  images: Image[];
}

export type Delivery = {
  id: number;
  body: string | null;
  canComment: boolean;
  canEdit: boolean;
  canFlag: boolean;
  canTag: boolean;
  createdAtUtc: Date;
  createdBy: string;
  editedAtUtc: Date | null;
  editedBy: string | null;
  flagCount: boolean;
  isAi: boolean;
  isDeleted: boolean;
  isDelivered: boolean;
  isDeliveryAccepted: boolean;
  isLocked: boolean;
  isNsfw: boolean;
  isNsfl: boolean;
  lockedAtUtc: Date | null;
  saveCount: boolean;
  title: string | null;
  upvoteCount: number;
  viewCount: number;
  images: Image[];
  comments: Comment[];
  tags: Tag[];
}

export type Comment = {
  id: number;
  askId: number | null;
  body: string;
  canEdit: boolean;
  canFlag: boolean;
  createdAtUtc: Date;
  createdBy: string;
  deliveryId: number | null;
  editedAtUtc: Date | null;
  editedBy: string | null;
  flagCount: number;
  isDeleted: boolean;
  isLocked: boolean;
  lockedAtUtc: Date | null;
  saveCount: number;
  threadId: number;
  upvoteCount: number;
};

export type Image = {
  url: string;
}

export type Tag = {
  id: number;
  type: string;
  value: string;
}
