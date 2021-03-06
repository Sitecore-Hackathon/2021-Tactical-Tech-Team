const getters = {
  isEditMode ({ context }) {
    return context && context.pageEditing
  },

  isPreviewMode ({ context }) {
    return context && context.pageState === 'preview'
  },

  isExperiencedMode (_, { isEditMode, isPreviewMode }) {
    return isEditMode || isPreviewMode
  }
}

export default getters
