export interface Person {
  id: string
  groupId: string
  surname: string
  name: string
  major: "wi" | "inf" | "mcd" | "et"
  gender: "m" | "f" | "d"
  age: number
}

export interface Group {
  id: string
  name: string
  member: Array<Person>
}
