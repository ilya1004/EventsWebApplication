
export interface UserInfoDTO {
  id: number,
  userName: string,
  email: string,
  name: string,
  surname: string,
  birthday: string,
}

export interface Event {
  id: number,
  title: string,
  description: string | null,
  participantsMaxCount: number,
  eventDateTime: string,
  image: string | null,
  category: EventCategory | null,
  place: EventPlace | null,
  participants: Participant[],
}

export interface EventWithRemainingPlacesDTO {
  id: number,
  title: string,
  description: string | null,
  eventDateTime: string,
  participantsMaxCount: number,
  placesRemain: number,
  placeName: string,
  categoryName: string | null,
}

export interface EventCategory {
  name: string,
  normalizedName: string,
}

export interface EventPlace {
  name: string,
  normalizedName: string,
}

export interface Participant {
  email: string,
  person: Person | null,
  eventId: number,
  event: Event | null,
  eventRegistrationDate: string,
}

export interface Person {
  name: string,
  surname: string,
  birthday: string,
}
